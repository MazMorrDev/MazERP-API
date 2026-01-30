using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IInventoryService
{
    public Task<List<Inventory>> GetInventoriesByWarehouseAsync(int id);
    public Task<Inventory> CreateInventoryAsync(CreateInventoryDto inventoryDto);
    public Task<Inventory> DeleteInventoryAsync(int id);
}
