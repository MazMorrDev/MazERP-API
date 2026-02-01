using MazErpBack.Context;
using MazErpBack.DTOs.Devolution;
using MazErpBack.Models;

namespace MazErpBack.Services.Implementation;

public class DevolutionService(AppDbContext context) : IDevolutionService
{
    private readonly AppDbContext _context = context;

    public Task<Devolution> CreateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDevolutionAsync(int devolutionId)
    {
        var devolution = await _context.Devolutions.FindAsync(devolutionId);
        if(devolution != null) _context.Devolutions.Remove(devolution);
    }

    public Task<Devolution> GetDevolutionByIdAsync(int devolutionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Devolution>> GetDevolutionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Devolution>> GetDevolutionsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Devolution>> GetDevolutionsByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public Task<Devolution> SoftDeleteDevolutionAsync(int devolutionID)
    {
        throw new NotImplementedException();
    }

    public Task<Devolution> UpdateDevolutionAsync(UpdateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }
}
