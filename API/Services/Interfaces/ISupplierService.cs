using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Utils;

namespace MazErpBack.Services.Interfaces;

public interface ISupplierService
{
    // Only avaible for admin pannel or backend operations
    public Task<Supplier> GetSupplierByIdAsync(int id);
    public Task DeleteSupplierAsync(int supplierId);

    // For common users
    public Task<PaginatedResult<SupplierDto>> GetSuppliersByWarehouseAsync(int warehouseId, int pageNumber, int pageSize);
    public Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto supplierDto);
    public Task<SupplierDto> UpdateSupplierAsync(int SupplierId, CreateSupplierDto supplierDto);
    public Task<bool> SoftDeleteSupplierAsync(int supplierId);
}
