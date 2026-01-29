using MazErpBack.Dtos.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IInventoryService
{
    public Task<List<Inventory>> GetInventoriesByWarehouseAsync(int id);
    public Task<Inventory> CreateInventoryAsync(CreateInventoryDto inventoryDto);
    public Task<Inventory> DeleteInventoryAsync(int id);
}
