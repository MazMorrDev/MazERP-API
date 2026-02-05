using MazErpBack.DTOs.Inventory;

namespace MazErpBack.Services.Interfaces;

public interface ISellPointInventoryService
{
    // This method has to ensure that if an user is already at the wf then update the values
    public Task<SellPointInventoryDto> AssignInventoryToSellPointAsync(CreateSellPointInventoryDto sellPointInventoryDto);
    public Task<bool> SoftDeleteSellPointInventoryAsync(DeleteSellPointInventoryDto sellPointInventoryDto);

    // For very old registries that we don't want
    public Task<bool> DeleteSellPointInventoryAsync(int sellId,DeleteSellPointInventoryDto sellPointInventoryDto);
}
