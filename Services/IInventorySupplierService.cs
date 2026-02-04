using MazErpBack.DTOs.Inventory;

namespace MazErpBack.Services;

public interface IInventorySupplierService
{
    // This method has to ensure that if an user is already at the wf then update the values
    public Task<InventorySupplierDto> AssignSupplierToInventoryAsync(InventorySupplierDto userWorkflowDto);
    public Task<bool> SoftDeleteInventorySupplierAsync(DeleteInventorySupplierDto userWorkflowDto);

    // For very old registries that we don't want
    public Task<bool> DeleteInventorySupplierAsync(DeleteInventorySupplierDto inventorySupplierDto);
}
