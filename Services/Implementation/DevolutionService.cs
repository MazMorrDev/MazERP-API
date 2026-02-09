using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class DevolutionService(AppDbContext context, DevolutionMapper mapper, ILogger<DevolutionService> logger) : IDevolutionService
{
    private readonly AppDbContext _context = context;
    private readonly DevolutionMapper _mapper = mapper;
    private readonly ILogger<DevolutionService> _logger = logger;

    public async Task<DevolutionDto> CreateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        var sell = await _context.Sells.FindAsync(devolutionDto.SellId);
        ArgumentNullException.ThrowIfNull(sell);

        var devolution = _mapper.MapDtoToModel(sell, devolutionDto);
        await _context.Devolutions.AddAsync(devolution);
        await _context.SaveChangesAsync();

        return _mapper.MapToDto(devolution);
    }

    public async Task<bool> DeleteDevolutionAsync(int devolutionId)
    {
        try
        {
            var devolution = await _context.Devolutions.FindAsync(devolutionId);
            if (devolution == null)
            {
                // logging
                return false;
            }

            _context.Devolutions.Remove(devolution);
            var result = await _context.SaveChangesAsync() > 0;

            if (result)
                _logger.LogInformation("Devolución {Id} eliminada", devolutionId);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar devolución {Id}", devolutionId);
            return false;
        }
    }

    public async Task<Devolution> GetDevolutionByIdAsync(int devolutionId)
    {
        try
        {
            var devolution = await _context.Devolutions.FindAsync(devolutionId);
            ArgumentNullException.ThrowIfNull(devolution);
            return devolution;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            throw;
        }

    }

    public async Task<List<Devolution>> GetDevolutionsAsync()
    {
        var devolutions = await _context.Devolutions.ToListAsync();
        ArgumentNullException.ThrowIfNull(devolutions);
        return devolutions;
    }

    public async Task<List<DevolutionDto>> GetDevolutionsByInventoryAsync(int inventoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<DevolutionDto>> GetDevolutionsBySellPointAsync(int sellPointId)
    {
        var movements = await _context.Movements.Where(m => m.SellPointId == sellPointId).ToListAsync();
        List<DevolutionDto> devolutionsDto = [];
        foreach (var movement in movements)
        {
            var devolutions = _mapper.MapListToDto(await _context.Devolutions.Where(d => d.SellId == movement.Id).ToListAsync());
            devolutionsDto.AddRange(devolutions);
        }
        return devolutionsDto;
    }

    public async Task<bool> SoftDeleteDevolutionAsync(int devolutionId)
    {
        var devolution = await _context.Devolutions.FindAsync(devolutionId);
        if (devolution == null) return false;

        devolution.IsActive = false;
        devolution.UpdatedAt = DateTimeOffset.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DevolutionDto> UpdateDevolutionAsync(int devolutionId, CreateDevolutionDto devolutionDto)
    {
        var devolution = await _context.Devolutions.FindAsync(devolutionId);
        ArgumentNullException.ThrowIfNull(devolution);

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
