using MazErpBack.DTOs.Movements;

namespace MazErpBack.Services;

public interface IBuyService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<BuyDto>> GetBuysAsync();
    public Task<BuyDto> GetBuyByIdAsync(int buyId);
    public Task<bool> DeleteBuyAsync(int buyId);

    // For common users
    public Task<List<BuyDto>> GetBuysByWorkflowAsync(int workflowId);
    public Task<List<BuyDto>> GetBuysByWarehouseAsync(int warehouseId);
    public Task<BuyDto> CreateBuyAsync(CreateBuyDto buyDto);
    public Task<BuyDto> UpdateBuyAsync(CreateBuyDto buyDto);
    public Task<bool> SoftDeleteBuyAsync(int buyID);
}
