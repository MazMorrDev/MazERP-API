using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class DevolutionService(AppDbContext context, DevolutionMapper mapper, ILogger<DevolutionService> logger, ISellService sellService) : IDevolutionService
{
    private readonly AppDbContext _context = context;
    private readonly DevolutionMapper _mapper = mapper;
    private readonly ILogger<DevolutionService> _logger = logger;
    private readonly ISellService _sellService = sellService;

    public async Task<DevolutionDto> CreateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        var sell = await _sellService.GetSellByIdAsync(devolutionDto.SellId);

        var devolution = _mapper.MapDtoToModel(sell, devolutionDto);
        await _context.Devolutions.AddAsync(devolution);
        await _context.SaveChangesAsync();

        return _mapper.MapToDto(devolution);
    }

    public async Task DeleteDevolutionAsync(int devolutionId)
    {
        try
        {
            var devolution = await GetDevolutionByIdAsync(devolutionId);
            _context.Devolutions.Remove(devolution);
            var result = await _context.SaveChangesAsync() > 0;

            if (result)
                _logger.LogInformation("Devolución {Id} eliminada", devolutionId);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar devolución {Id}", devolutionId);
            throw;
        }
    }

    public async Task<Devolution> GetDevolutionByIdAsync(int devolutionId)
    {
        try
        {
            var devolution = await _context.Devolutions.FindAsync(devolutionId);
            if (devolution == null || !devolution.IsActive) throw new KeyNotFoundException($"Devolution with id: {devolutionId} not found");
            return devolution;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            throw;
        }

    }

    public async Task<List<DevolutionDto>> GetDevolutionsByInventoryAsync(int inventoryId)
    {
        var sellPointsInventories = await _context.SellPointInventories.Where(spi => spi.InventoryId == inventoryId).ToListAsync();
        List<DevolutionDto> devolutionsDto = [];
        foreach (var spi in sellPointsInventories)
        {
            var sellPoints = await _context.SellPoints.Where(sp => sp.Id.Equals(spi.SellPointId) && sp.IsActive).ToListAsync();
            foreach (var sp in sellPoints)
            {
                devolutionsDto.AddRange(await GetDevolutionsBySellPointAsync(sp.Id));
            }
        }
        return devolutionsDto;
    }

    public async Task<List<DevolutionDto>> GetDevolutionsBySellPointAsync(int sellPointId)
    {
        var sells = await _context.Sells.Where(m => m.SellPointId == sellPointId && m.Movement.IsActive).ToListAsync();
        List<DevolutionDto> devolutionsDto = [];
        foreach (var s in sells)
        {
            var devolutions = _mapper.MapListToDto(await _context.Devolutions.Where(d => d.SellId == s.MovementId && d.IsActive).ToListAsync());
            devolutionsDto.AddRange(devolutions);
        }
        return devolutionsDto;
    }

    public async Task<bool> SoftDeleteDevolutionAsync(int devolutionId)
    {
        try
        {
            var devolution = await GetDevolutionByIdAsync(devolutionId);
            devolution.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<DevolutionDto> UpdateDevolutionAsync(int devolutionId, CreateDevolutionDto devolutionDto)
    {
        var devolution = await GetDevolutionByIdAsync(devolutionId);

        devolution.SellId = devolutionDto.SellId;
        devolution.Reason = devolutionDto.Reason;
        devolution.RefundAmount = devolutionDto.RefundAmount;
        devolution.Notes = devolutionDto.Notes;
        devolution.DevolutionStatus = devolutionDto.DevolutionStatus;
        devolution.DevolutionActionTake = devolutionDto.DevolutionActionTake;
        devolution.DevolutionDate = devolutionDto.Date;
        await _context.SaveChangesAsync();

        return _mapper.MapToDto(devolution);
    }
}
