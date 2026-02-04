using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ISellService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<SellDto>> GetSellsAsync();
    public Task<SellDto> GetSellById(int sellId);
    public Task<bool> DeleteSellAsync(int sellId);

    // For common users
    public Task<List<SellDto>> GetSellsByWorkflowAsync(int workflowId);
    public Task<List<SellDto>> GetSellsByWarehouseAsync(int warehouseId);
    public Task<SellDto> CreateSellAsync(CreateSellDto sellDto);
    public Task<SellDto> UpdateSellAsync(CreateSellDto sellDto);
    public Task<bool> SoftDeleteSellAsync(int sellID);
}
