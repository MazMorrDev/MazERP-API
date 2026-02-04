using MazErpBack.DTOs.Inventory;

namespace MazErpBack.Services;

public interface IInventoryService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<InventoryDto>> GetInventoriesAsync();
    public Task<InventoryDto> GetInventoryByIdAsync();
    public Task<bool> DeleteInventoryAsync(int id);

    // For common users
    public Task<List<InventoryDto>> GetInventoriesByWarehouseAsync(int inventoryId);
    public Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto inventoryDto);
    public Task<InventoryDto> UpdateInventoryAsync(CreateInventoryDto inventoryDto);
    public Task<bool> SoftDeleteInventoryAsync(int inventoryId);

}
