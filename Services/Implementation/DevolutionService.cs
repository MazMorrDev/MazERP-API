using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class DevolutionService(AppDbContext context, DevolutionMapper mapper, ILogger logger) : IDevolutionService
{
    private readonly AppDbContext _context = context;
    private readonly DevolutionMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<DevolutionDto> CreateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        var sell = await _context.Sells.FindAsync(devolutionDto.SellId);
        ArgumentNullException.ThrowIfNull(sell);

        var devolution = new Devolution
        {
            SellId = devolutionDto.SellId,
            


            Sell = sell,
        }
    }

    public async Task<bool> DeleteDevolutionAsync(int devolutionId)
    {
        try
        {
            var devolution = await _context.Devolutions.FindAsync(devolutionId);
            if (devolution == null)
            {
                _logger.LogWarning("Devolución {Id} no encontrada para eliminar", devolutionId);
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
            throw e;
        }

    }

    public async Task<List<Devolution>> GetDevolutionsAsync()
    {
        var devolutions = await _context.Devolutions.ToListAsync();
        ArgumentNullException.ThrowIfNull(devolutions);
        return devolutions;
    }

    public async Task<List<DevolutionDto>> GetDevolutionsByWarehouseAsync(int warehouseId)
    {
        // TODO: we have to use a lot of services to know this
        throw new NotImplementedException();
    }

    public async Task<List<DevolutionDto>> GetDevolutionsByCompanyAsync(int companyId)
    {
        // TODO: we have to use a lot of services to know this
        throw new NotImplementedException();
    }

    public async Task<bool> SoftDeleteDevolutionAsync(int devolutionId)
    {
        var devolution = await _context.Devolutions.FindAsync(devolutionId);
        if (devolution == null) return false;

        devolution.IsActive = false;
        devolution.UpdatedAt = DateTimeOffset.Now;
        return true;
    }

    public async Task<DevolutionDto> UpdateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }
}
