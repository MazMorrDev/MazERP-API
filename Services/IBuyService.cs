using MazErpBack.DTOs.Movement;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IBuyService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Product>> GetBuysAsync();
    public Task<Product> GetBuyByIdAsync(int buyId);
    public Task DeleteBuyAsync(int buyId);

    // For common users
    public Task<List<Product>> GetBuysByWorkflowAsync(int workflowId);
    public Task<List<Product>> GetBuysByWarehouseAsync(int warehouseId);
    public Task<Product> CreateBuyAsync(CreateBuyDto buyDto);
    public Task<Product> UpdateBuyAsync(UpdateBuyDto buyDto);
    public Task<Product> SoftDeleteBuyAsync(int buyID);
}
