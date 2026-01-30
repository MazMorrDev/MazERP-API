using MazErpBack.DTOs.Devolution;
using MazErpBack.Models;

namespace MazErpBack.Services.Implementation;

public class DevolutionService : IDevolutionService
{
    public Task<Devolution> CreateDevolutionAsync(CreateDevolutionDto devolutionDto)
    {
        throw new NotImplementedException();
    }

    public Task<Devolution> DeleteDevolutionAsync(int DevolutionId)
    {
        throw new NotImplementedException();
    }

    public Task<Devolution> GetDevolutionById(int DevolutionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Devolution>> GetDevolutionsAsync()
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
