using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class BuyMapper(ILogger<BuyMapper> logger)
{
    private readonly ILogger<BuyMapper> _logger = logger;

    public BuyDto? MapToDto(Movement movement, Buy buy)
    {
        if (movement.Id != buy.MovementId)
        {
            _logger.LogError("se está intentando mapear un Buy con un id que no es el mismo del Movement");
            return null;
        }

        return new BuyDto
        {
            MovementId = movement.Id,
            UserId = movement.UserId,
            SellPointId = movement.SellPointId,
            Description = movement.Description,
            MovementType = movement.MovementType,
            Quantity = movement.Quantity,
            Currency = movement.Currency,
            MovementDate = movement.MovementDate,
            SupplierId = buy.SupplierId,
            DeliveryStatus = buy.DeliveryStatus,
            UnitaryCost = buy.UnitaryCost,
            ReceivedQuantity = buy.ReceivedQuantity
        };
    }

    public List<BuyDto>? MapListToDto(List<Movement> movements, List<Buy> buys)
    {
        List<BuyDto> buysDto = [];
        foreach (var buy in buys)
        {
            foreach (var movement in movements)
            {
                var buyDto = MapToDto(movement, buy);
                if (buyDto == null) return null;
                buysDto.Add(buyDto);
            }
        }
        return buysDto;
    }
}
