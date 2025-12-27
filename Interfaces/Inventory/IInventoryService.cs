namespace MazErpBack;

public interface IInventoryService
{
    public Task<List<Inventory>> GetInventoriesByWarehouseAsync(int id);
    public Task<Inventory> CreateInventory(CreateInventoryDto inventoryDto);
    public Task<Inventory> DeleteInventory(int id);
}
