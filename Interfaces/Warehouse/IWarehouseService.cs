namespace MazErpBack;

public interface IWarehouseService
{
    public Task<Warehouse> CreateWarehouse(CreateWarehouseDto warehouseDto);
    public Task<Warehouse> DeleteWarehouse(DeleteWarehouseDto warehouseDto);
}
