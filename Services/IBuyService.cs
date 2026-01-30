using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IBuyService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Buy>> GetDevolutionsAsync();
    public Task<Buy> GetDevolutionById(int DevolutionId);
    public Task<Buy> DeleteDevolutionAsync(int DevolutionId);

    // For common users
    public Task<List<Buy>> GetDevolutionsByWorkflowAsync(int workflowId);
    public Task<List<Buy>> GetDevolutionsByWarehouseAsync(int warehouseId);
    public Task<Buy> CreateDevolutionAsync(CreateDevolutionDto devolutionDto);
    public Task<Buy> UpdateDevolutionAsync(UpdateDevolutionDto devolutionDto);
    public Task<Buy> SoftDeleteDevolutionAsync(int devolutionID);
}
