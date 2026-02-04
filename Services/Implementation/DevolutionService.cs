using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Implementation;

public class DevolutionService(AppDbContext context) : IDevolutionService
{
    private readonly AppDbContext _context = context;

    public async Task<DevolutionDto> CreateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteDevolutionAsync(int devolutionId)
    {
        var devolution = await _context.Devolutions.FindAsync(devolutionId);
        if (devolution != null)
        {
            _context.Devolutions.Remove(devolution);
            return true;
        }
        return false;
    }

    public async Task<DevolutionDto> GetDevolutionByIdAsync(int devolutionId)
    {
        var devolution = await _context.Devolutions.FindAsync(devolutionId);
        return
    }

    public async Task<List<DevolutionDto>> GetDevolutionsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<DevolutionDto>> GetDevolutionsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<DevolutionDto>> GetDevolutionsByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SoftDeleteDevolutionAsync(int devolutionId)
    {
        throw new NotImplementedException();
    }

    public async Task<DevolutionDto> UpdateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }
}
