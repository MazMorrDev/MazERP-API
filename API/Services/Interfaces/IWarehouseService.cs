using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpAPI.Utils;

namespace MazErpAPI.Services.Interfaces;

public interface IWarehouseService
{
    // For admin pannel or back operations
    public Task<Warehouse> GetWarehouseByIdAsync(int warehouseId);
    public Task DeleteWarehouseAsync(int id);

    // For common users
    public Task<PaginatedResult<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId, int pageNumber, int pageSize);
    public Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto warehouseDto);
    public Task<WarehouseDto> UpdateWarehouseAsync(int WarehouseId, CreateWarehouseDto warehouseDto);
    public Task<bool> SoftDeleteWarehouseAsync(int warehouseId);
}
