using MazErpBack.DTOs.Warehouse;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IWarehouseService
{
    public Task<Warehouse> CreateWarehouse(CreateWarehouseDto warehouseDto);
    public Task<Warehouse> DeleteWarehouse(int id);
    public Task<List<Warehouse>> GetWarehousesByWorkflowAsync(int workflowId);
}
