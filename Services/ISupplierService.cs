using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ISupplierService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Supplier>> GetSuppliersAsync();
    public Task<Supplier> GetSupplierById(int id);
    public Task<bool> DeleteSupplierAsync(int supplierId);

    // For common users
    public Task<List<SupplierDto>> GetSuppliersByCompanyAsync(int companyId);
    public Task<List<SupplierDto>> GetSuppliersByWarehouseAsync(int warehouseId);
    public Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto supplierDto);
    public Task<SupplierDto> UpdateSupplierAsync(CreateSupplierDto supplierDto);
    public Task<bool> SoftDeleteSupplierAsync(int supplierId);
}
