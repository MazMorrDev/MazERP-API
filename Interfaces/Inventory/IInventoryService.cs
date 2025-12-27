namespace MazErpBack;

public interface IInventoryService
{
    public Task<List<Inventory>> GetInventoriesByWarehouseAsync();
    public Task<Inventory> CreateInventory();
    public Task<Inventory> DeleteInventory();
}
