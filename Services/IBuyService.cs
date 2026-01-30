using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IBuyService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Buy>> GetBuysAsync();
    public Task<Buy> GetBuyById(int buyId);
    public Task<Buy> DeleteBuyAsync(int buyId);

    // For common users
    public Task<List<Buy>> GetBuysByWorkflowAsync(int workflowId);
    public Task<List<Buy>> GetBuysByWarehouseAsync(int warehouseId);
    public Task<Buy> CreateBuyAsync(CreateBuyDto buyDto);
    public Task<Buy> UpdateBuyAsync(UpdateBuyDto buyDto);
    public Task<Buy> SoftDeleteBuyAsync(int buyID);
}
