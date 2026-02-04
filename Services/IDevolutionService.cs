using MazErpBack.DTOs.Movements;

namespace MazErpBack.Services;

public interface IDevolutionService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<DevolutionDto>> GetDevolutionsAsync();
    public Task<DevolutionDto> GetDevolutionByIdAsync(int devolutionId);
    public Task<bool> DeleteDevolutionAsync(int devolutionId);

    // For common users
    public Task<List<DevolutionDto>> GetDevolutionsByWorkflowAsync(int workflowId);
    public Task<List<DevolutionDto>> GetDevolutionsByWarehouseAsync(int warehouseId);
    public Task<DevolutionDto> CreateDevolutionAsync(CreateDevolutionDto devolutionDto);
    public Task<DevolutionDto> UpdateDevolutionAsync(CreateDevolutionDto devolutionDto);
    public Task<bool> SoftDeleteDevolutionAsync(int devolutionId);
}
