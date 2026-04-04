using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpBack.Utils;

namespace MazErpAPI.Services.Interfaces;

public interface IInventoryService
{
    // Only avaible for admin pannel or backend operations
    public Task<Inventory> GetInventoryByIdAsync(int inventoryId);
    public Task DeleteInventoryAsync(int inventoryId);

    // For common users
    public Task<PaginatedResult<InventoryDto>> GetInventoriesByWarehouseAsync(int inventoryId, int pageNumber, int pageSize);
    public Task<PaginatedResult<InventoryDto>> GetInventoriesByCompanyAsync(int companyId, int pageNumber, int pageSize);
    public Task<InventoryDto> CreateInventoryAndProductAsync(CreateInventoryAndProductDto inventoryDto);
    public Task<InventoryDto> CreateInventoryByExistentProductAsync(CreateInventoryByExistentProductDto inventoryDto);
    public Task<InventoryDto> UpdateInventoryAndProductAsync(int inventoryId, UpdateInventoryProductDto inventoryDto);
    public Task<bool> SoftDeleteInventoryAsync(int inventoryId);

}
