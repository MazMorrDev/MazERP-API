using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class InventoryMapper
{
    public InventoryDto MapToDto(Inventory inventory, Product product)
    {
        return new InventoryDto
        {
            InventoryId = inventory.Id,
            WarehouseId = inventory.WarehouseId,
            ProductId = inventory.ProductId,
            Stock = inventory.Stock,
            BaseDiscount = inventory.BaseDiscount,
            BasePrice = inventory.BasePrice,
            AverageCost = inventory.AverageCost,
            AlertStock = inventory.AlertStock,
            WarningStock = inventory.WarningStock,
        };
    }

    public List<InventoryDto> MapListToDto(List<Inventory> inventories)
    {
        List<InventoryDto> inventoriesDto = [];
        foreach (var inventory in inventories)
        {
            inventoriesDto.Add(MapToDto(inventory));
        }
        return inventoriesDto;
    }

    public CreateInventoryExistentProductDto MapNewToExistent(Product product, CreateInventoryNewProductDto dto)
    {
        return new CreateInventoryExistentProductDto
        {
            WarehouseId = dto.WarehouseId,
            ProductId = product.Id,
            Stock = dto.Stock,
            BaseDiscount =dto.BaseDiscount,
            BasePrice = dto.BasePrice,
            AverageCost =dto.AverageCost,
            AlertStock = dto.AlertStock,
            WarningStock = dto.WarningStock
        };
    }

    public Inventory MapDtoToModel(Warehouse warehouse, Product product, CreateInventoryExistentProductDto inventoryDto)
    {
        return new Inventory
        {
            WarehouseId = inventoryDto.WarehouseId,
            ProductId = inventoryDto.ProductId,
            Stock = inventoryDto.Stock,
            BasePrice = inventoryDto.BasePrice,
            BaseDiscount = inventoryDto.BaseDiscount,
            AverageCost = inventoryDto.AverageCost,
            AlertStock = inventoryDto.AlertStock,
            WarningStock = inventoryDto.WarningStock,
            Warehouse = warehouse,
            Product = product
        };
    }
}
