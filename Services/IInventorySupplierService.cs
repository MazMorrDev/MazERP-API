using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IInventorySupplierService
{
    // This method has to ensure that if an user is already at the wf then update the values
    public Task<InventorySupplier> AssignSupplierToInventoryAsync(AssignSupplierToInventoryDto userWorkflowDto);
    public Task<InventorySupplier> SoftDeleteInventorySupplierAsync(DeleteInventorySupplierDto userWorkflowDto);

    // For very old registries that we don't want
    public Task DeleteInventorySupplierAsync(DeleteInventorySupplierDto inventorySupplierDto);
}
