using MazErpBack.DTOs.Warehouse;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IWarehouseService
{
    public Task<Warehouse> CreateWarehouseAsync(CreateWarehouseDto warehouseDto);
    public Task<Warehouse> DeleteWarehouseAsync(int id);
    public Task<List<Warehouse>> GetWarehousesByWorkflowAsync(int workflowId);
}
