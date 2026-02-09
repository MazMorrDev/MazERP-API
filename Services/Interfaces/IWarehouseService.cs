using MazErpBack.DTOs.Inventory;

namespace MazErpBack.Services.Interfaces;

public interface IWarehouseService
{
    // For admin pannel or back operations
    public Task<WarehouseDto> GetWarehouseByIdAsync(int warehouseId);
    public Task<bool> DeleteWarehouseAsync(int id);

    // For common users
    public Task<List<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId);
    public Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto warehouseDto);
    public Task<WarehouseDto> UpdateWarehouseAsync(int WarehouseId, CreateWarehouseDto warehouseDto);
    public Task<bool> SoftDeleteWarehouseAsync(int warehouseId);
}
