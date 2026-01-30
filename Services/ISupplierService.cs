using MazErpBack.Dtos.Supplier;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ISupplierService
{
    // Only avaible for admin pannel
    public Task<List<Supplier>> GetSuppliersAsync();
    public Task<Supplier> DeleteSupplierAsync(int supplierId);

    // For common users
    public Task<List<Supplier>> GetSuppliersByWorkflowAsync(int workflowId);
    public Task<Supplier> CreateSupplierAsync(CreateSupplierDto supplierDto);
    public Task<Supplier> UpdateSupplierAsync(UpdateSupplierDto supplierDto);
    public Task<Supplier> SoftDeleteSupplierAsync(int supplierId);
}
