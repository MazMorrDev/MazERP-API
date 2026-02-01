using MazErpBack.DTOs.Supplier;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ISupplierService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Supplier>> GetSuppliersAsync();
    public Task<Supplier> GetSupplierById(int id);
    public Task DeleteSupplierAsync(int supplierId);

    // For common users
    public Task<List<Supplier>> GetSuppliersByWorkflowAsync(int workflowId);
    public Task<List<Supplier>> GetSuppliersByWarehouseAsync(int warehouseId);
    public Task<Supplier> CreateSupplierAsync(CreateSupplierDto supplierDto);
    public Task<Supplier> UpdateSupplierAsync(UpdateSupplierDto supplierDto);
    public Task<Supplier> SoftDeleteSupplierAsync(int supplierId);
}
