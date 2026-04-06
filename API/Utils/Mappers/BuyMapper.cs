using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using Microsoft.Extensions.Logging;

namespace MazErpAPI.Utils.Mappers;

public class BuyMapper(ILogger<BuyMapper> logger)
{
    private readonly ILogger<BuyMapper> _logger = logger;

    public BuyDto MapToDto(Movement movement, Buy buy)
    {
        return new BuyDto
        {
            MovementId = movement.Id,
            InventoryId = buy.InventoryId,
            UserId = movement.UserId,
            Description = movement.Description,
            Quantity = movement.Quantity,
            Currency = movement.Currency,
            MovementDate = movement.MovementDate,
            SupplierId = buy.SupplierId,
            DeliveryStatus = buy.DeliveryStatus,
            UnitaryCost = buy.UnitaryCost,
            ReceivedQuantity = buy.ReceivedQuantity
        };
    }

    public Buy MapBuy(Movement movement, Supplier supplier, Inventory inventory, CreateBuyDto buyDto)
    {
        return new Buy
        {
            MovementId = movement.Id,
            InventoryId = buyDto.InventoryId,
            SupplierId = buyDto.SupplierId,
            DeliveryStatus = buyDto.DeliveryStatus,
            UnitaryCost = buyDto.UnitaryCost,
            ReceivedQuantity = buyDto.ReceivedQuantity,
            Movement = movement,
            Supplier = supplier,
            Inventory = inventory
        };
    }

    public Movement MapMovement(User user, CreateBuyDto buyDto)
    {
        return new Movement
        {
            UserId = buyDto.UserId,
            Description = buyDto.Description,
            Quantity = buyDto.Quantity,
            Currency = buyDto.Currency,
            MovementDate = buyDto.MovementDate,
            User = user
        };
    }

    public List<BuyDto> MapListToDto(List<Movement> movements, List<Buy> buys)
    {
        List<BuyDto> buysDto = [];
        foreach (var buy in buys)
        {
            foreach (var movement in movements)
            {
                var buyDto = MapToDto(movement, buy);
                buysDto.Add(buyDto);
            }
        }
        return buysDto;
    }
}
