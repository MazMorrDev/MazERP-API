using MazErpBack.DTOs.Warehouse;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IWarehouseService
{
    // For admin pannel or back operations
    public Task DeleteWarehouseAsync(int id);

    // For common users
    public Task<Warehouse> CreateWarehouseAsync(CreateWarehouseDto warehouseDto);
    public Task<List<Warehouse>> GetWarehousesByWorkflowAsync(int workflowId);
}
