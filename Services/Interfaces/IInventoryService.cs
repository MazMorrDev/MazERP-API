using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IInventoryService
{
    // Only avaible for admin pannel or backend operations
    public Task<Inventory> GetInventoryByIdAsync(int inventoryId);
    public Task DeleteInventoryAsync(int inventoryId);

    // For common users
    public Task<List<InventoryDto>> GetInventoriesByWarehouseAsync(int inventoryId);
    public Task<List<InventoryDto>> GetInventoriesByCompanyAsync(int companyId);
    public Task<InventoryDto> CreateInventoryAndProductAsync(CreateInventoryAndProductDto inventoryDto);
    public Task<InventoryDto> CreateInventoryByExistentProductAsync(CreateInventoryByExistentProductDto inventoryDto);
    public Task<InventoryDto> UpdateInventoryAndProductAsync(int inventoryId, UpdateInventoryProductDto inventoryDto);
    public Task<bool> SoftDeleteInventoryAsync(int inventoryId);

}
