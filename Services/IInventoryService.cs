using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IInventoryService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Inventory>> GetInventoriesAsync();
    public Task<Inventory> GetInventoryByIdAsync();
    public Task DeleteInventoryAsync(int id);

    // For common users
    public Task<Inventory> CreateInventoryAsync(CreateInventoryDto inventoryDto);
    public Task<List<Inventory>> GetInventoriesByWarehouseAsync(int inventoryId);
    public Task<Inventory> UpdateInventoryAsync(UpdateInventoryDto inventoryDto);
    public Task<Inventory> SoftDeleteInventoryAsync(int inventoryId);

}
