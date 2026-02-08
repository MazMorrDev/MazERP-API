using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IInventoryService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Inventory>> GetInventoriesAsync();
    public Task<Inventory> GetInventoryByIdAsync(int inventoryId);
    public Task<bool> DeleteInventoryAsync(int inventoryId);

    // For common users
    public Task<List<InventoryDto>> GetInventoriesByWarehouseAsync(int inventoryId);
    public Task<List<InventoryDto>> GetInventoriesByCompanyAsync(int companyId);
    public Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto inventoryDto);
    public Task<InventoryDto> UpdateInventoryAsync(int inventoryId, CreateInventoryDto inventoryDto);
    public Task<bool> SoftDeleteInventoryAsync(int inventoryId);

}
