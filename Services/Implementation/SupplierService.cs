using MazErpBack.Dtos.Supplier;
using MazErpBack.Models;

namespace MazErpBack.Services.Implementation;

public class SupplierService : ISupplierService
{
    public Task<Supplier> CreateSupplierAsync(CreateSupplierDto supplierDto)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier> DeleteSupplierAsync(int supplierId)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier> GetSupplierById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Supplier>> GetSuppliersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Supplier>> GetSuppliersByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier> SoftDeleteSupplierAsync(int supplierId)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier> UpdateSupplierAsync(UpdateSupplierDto supplierDto)
    {
        throw new NotImplementedException();
    }
}
