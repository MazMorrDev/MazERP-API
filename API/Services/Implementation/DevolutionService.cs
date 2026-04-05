using MazErpAPI.Context;
using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpAPI.Services.Implementation;

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

    public async Task<PaginatedResult<DevolutionDto>> GetDevolutionsByInventoryAsync(
        int inventoryId, int pageNumber, int pageSize)
    {
        // Consulta única que obtiene todas las devoluciones del inventario
        var query = _context.Devolutions
            .Include(de => de.Sell)
                .ThenInclude(s => s.SellPoint)
            .Where(de => de.Sell.SellPointId != 0
                         && de.Sell.Movement.IsActive
                         && de.Sell.SellPoint.IsActive
                         && _context.SellPointInventories.Any(spi =>
                             spi.SellPointId == de.Sell.SellPointId &&
                             spi.InventoryId == inventoryId));

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var devolutionsDto = items.Select(_mapper.MapToDto).ToList();

        return new PaginatedResult<DevolutionDto>(devolutionsDto, totalCount, pageNumber, pageSize);
    }

    public async Task<PaginatedResult<DevolutionDto>> GetDevolutionsBySellPointAsync(int sellPointId, int pageNumber, int pageSize)
    {
        var query = _context.Devolutions.Include(de => de.Sell).Where(de => de.Sell.SellPointId == sellPointId && de.Sell.Movement.IsActive);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var devolutionsDto = items.Select(_mapper.MapToDto).ToList();
        return new PaginatedResult<DevolutionDto>(devolutionsDto, totalCount, pageNumber, pageSize);
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
