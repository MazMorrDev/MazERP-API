using MazErpBack.Context;
using MazErpBack.DTOs.Devolution;
using MazErpBack.Models;

namespace MazErpBack.Services.Implementation;

public class DevolutionService(AppDbContext context) : IDevolutionService
{
    private readonly AppDbContext _context = context;

    public Task<Movement> CreateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDevolutionAsync(int devolutionId)
    {
        var devolution = await _context.Devolutions.FindAsync(devolutionId);
        if(devolution != null) _context.Devolutions.Remove(devolution);
    }

    public Task<Movement> GetDevolutionByIdAsync(int devolutionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Movement>> GetDevolutionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Movement>> GetDevolutionsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Movement>> GetDevolutionsByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public Task<Movement> SoftDeleteDevolutionAsync(int devolutionID)
    {
        throw new NotImplementedException();
    }

    public Task<Movement> UpdateDevolutionAsync(UpdateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }
}
