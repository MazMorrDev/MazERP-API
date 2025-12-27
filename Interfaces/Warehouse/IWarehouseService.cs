namespace MazErpBack;

public interface IWarehouseService
{
    public Task<Warehouse> CreateWarehouse(CreateWarehouseDto warehouseDto);
    public Task<Warehouse> DeleteWarehouse(DeleteWarehouseDto warehouseDto);
    public Task<List<Warehouse>> GetWarehousesByWorkflowAsync(GetWarehousesByWfDto getWarehousesByWfDto);
}
