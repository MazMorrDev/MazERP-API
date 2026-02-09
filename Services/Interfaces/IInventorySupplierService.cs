using MazErpBack.DTOs.Inventory;

namespace MazErpBack.Services.Interfaces;

public interface IInventorySupplierService
{
    // This method has to ensure that if an inventory is already at the sellPoint then update the values
    public Task<InventorySupplierDto> AssignSupplierToInventoryAsync(InventorySupplierDto userWorkflowDto);
    public Task SoftDeleteInventorySupplierAsync(DeleteInventorySupplierDto userWorkflowDto);

    // For very old registries that we don't want
    public Task DeleteInventorySupplierAsync(DeleteInventorySupplierDto inventorySupplierDto);
}
