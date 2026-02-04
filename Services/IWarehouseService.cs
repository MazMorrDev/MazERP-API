using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IWarehouseService
{
    // For admin pannel or back operations
    public Task<List<Warehouse>> GetWarehousesAsync();
    public Task<Warehouse> GetWarehouseByIdAsync(int warehouseId);
    public Task DeleteWarehouseAsync(int id);

    // For common users
    public Task<List<Warehouse>> GetWarehousesByWorkflowAsync(int workflowId);
    public Task<Warehouse> CreateWarehouseAsync(CreateWarehouseDto warehouseDto);
    public Task<Warehouse> UpdateWarehouseAsync(CreateWarehouseDto warehouseDto);
    public Task<Warehouse> SoftDeleteWarehouseAsync(int warehouseId);
}
