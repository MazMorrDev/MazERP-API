using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;

namespace MazErpAPI.Utils.Mappers;

public class SellPointMapper
{
    public SellPointDto MapToDto(SellPoint sellPoint)
    {
        return new SellPointDto
        {
            Id = sellPoint.Id,
            Name = sellPoint.Name,
            Description = sellPoint.Description,
            Location = sellPoint.Location
        };
    }

    public SellPoint MapDtoToModel(CreateSellPointDto sellPointDto)
    {
        return new SellPoint
        {
            Name = sellPointDto.Name,
            Description = sellPointDto.Description,
            Location = sellPointDto.Location,
        };
    }

    public List<SellPointDto> MapListToDto(List<SellPoint> sellPoints)
    {
        List<SellPointDto> sellPointsDto = [];
        foreach (var sellPoint in sellPoints)
        {
            sellPointsDto.Add(MapToDto(sellPoint));
        }
        return sellPointsDto;
    }

    public SellPointInventoryDto MapSellPointInventoryDto(SellPointInventory sellpointInventory)
    {
        return new SellPointInventoryDto
        {
            SellPointId = sellpointInventory.SellPointId,
            InventoryId = sellpointInventory.InventoryId,
            SellPrice = sellpointInventory.SellPrice,
            Discount = sellpointInventory.Discount,
            Stock = sellpointInventory.Stock,
            WarningStock = sellpointInventory.WarningStock,
            AlertStock = sellpointInventory.AlertStock
        };
    }

        public SellPointInventory MapSellPointInventory(AssignInventoryToSellPointDto dto, Inventory inventory, SellPoint sellPoint)
    {
        return new SellPointInventory
        {
            SellPointId = dto.SellPointId,
            InventoryId = dto.InventoryId,
            SellPrice = dto.SellPrice,
            Discount = dto.Discount,
            Stock = dto.Stock,
            WarningStock = dto.WarningStock,
            AlertStock = dto.AlertStock,
            SellPoint = sellPoint,
            Inventory = inventory
        };
    }
}
