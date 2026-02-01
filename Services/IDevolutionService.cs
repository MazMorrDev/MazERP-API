using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IDevolutionService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Movement>> GetDevolutionsAsync();
    public Task<Movement> GetDevolutionByIdAsync(int devolutionId);
    public Task DeleteDevolutionAsync(int devolutionId);

    // For common users
    public Task<List<Movement>> GetDevolutionsByWorkflowAsync(int workflowId);
    public Task<List<Movement>> GetDevolutionsByWarehouseAsync(int warehouseId);
    public Task<Movement> CreateDevolutionAsync(CreateDevolutionDto devolutionDto);
    public Task<Movement> UpdateDevolutionAsync(UpdateDevolutionDto devolutionDto);
    public Task<Movement> SoftDeleteDevolutionAsync(int devolutionID);
}
