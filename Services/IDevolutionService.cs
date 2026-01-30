using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IDevolutionService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Devolution>> GetDevolutionsAsync();
    public Task<Devolution> GetDevolutionById(int DevolutionId);
    public Task<Devolution> DeleteDevolutionAsync(int DevolutionId);

    // For common users
    public Task<List<Devolution>> GetDevolutionsByWorkflowAsync(int workflowId);
    public Task<Devolution> CreateDevolutionAsync(CreateDevolutionDto devolutionDto);
    public Task<Devolution> UpdateDevolutionAsync(UpdateDevolutionDto devolutionDto);
    public Task<Devolution> SoftDeleteDevolutionAsync(int devolutionID);
}
