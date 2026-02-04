using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ISellService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Sell>> GetSellsAsync();
    public Task<Sell> GetSellById(int sellId);
    public Task DeleteSellAsync(int sellId);

    // For common users
    public Task<List<Sell>> GetSellsByWorkflowAsync(int workflowId);
    public Task<List<Sell>> GetSellsByWarehouseAsync(int warehouseId);
    public Task<Sell> CreateSellAsync(CreateSellDto sellDto);
    public Task<Sell> UpdateSellAsync(CreateSellDto sellDto);
    public Task<Sell> SoftDeleteSellAsync(int sellID);
}
