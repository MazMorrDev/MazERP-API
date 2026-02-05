using MazErpBack.DTOs.Inventory;

namespace MazErpBack.Services;

public interface ISellPointInventoryService
{
    // This method has to ensure that if an user is already at the wf then update the values
    public Task<SellPointInventoryDto> AssignUserWithRoleToCompanyAsync(SellPointInventoryDto sellPointInventoryDto);
    public Task<bool> SoftDeleteUserCompanyAsync(DeleteSellPointInventoryDto sellPointInventoryDto);

    // For very old registries that we don't want
    public Task<bool> DeleteUserCompanyAsync(DeleteSellPointInventoryDto sellPointInventoryDto);
}
