using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IBuyService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Buy>> GetBuysAsync();
    public Task<Buy> GetBuyByIdAsync(int buyId);
    public Task<bool> DeleteBuyAsync(int buyId);

    // For common users
    public Task<List<BuyDto>> GetBuysByWorkflowAsync(int workflowId);
    public Task<List<BuyDto>> GetBuysByWarehouseAsync(int warehouseId);
    public Task<BuyDto> CreateBuyAsync(CreateBuyDto buyDto);
    public Task<BuyDto> UpdateBuyAsync(CreateBuyDto buyDto);
    public Task<bool> SoftDeleteBuyAsync(int buyID);
}
