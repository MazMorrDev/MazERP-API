using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;

namespace MazErpBack.Utils.Mappers;

public class SellMapper(ILogger<SellMapper> logger)
{
    private readonly ILogger<SellMapper> _logger = logger;

    public SellDto MapToDto(Movement movement, Sell sell)
    {
        return new SellDto
        {
            MovementId = movement.Id,
            UserId = movement.UserId,
            SellPointId = sell.SellPointId,
            Description = movement.Description,
            Quantity = movement.Quantity,
            Currency = movement.Currency,
            MovementDate = movement.MovementDate,
            DiscountPercentage = sell.DiscountPercentage,
            PaymentStatus = sell.PaymentStatus,
            SaleType = sell.SaleType,
            SellerNotes = sell.SellerNotes
        };
    }

    public Sell MapSell(Movement movement, SellPoint sellPoint, CreateSellDto SellDto)
    {
        return new Sell
        {
            MovementId = movement.Id,
            SellPointId = SellDto.SellPointId,
            DiscountPercentage = SellDto.DiscountPercentage,
            PaymentStatus = SellDto.PaymentStatus,
            SaleType = SellDto.SaleType,
            SellerNotes = SellDto.SellerNotes,
            Movement = movement,
            SellPoint = sellPoint
        };
    }

    public Movement MapMovement(User user, CreateSellDto SellDto)
    {
        return new Movement
        {
            UserId = SellDto.UserId,
            Description = SellDto.Description,
            Quantity = SellDto.Quantity,
            Currency = SellDto.Currency,
            MovementDate = SellDto.MovementDate,
            User = user
        };
    }

    public List<SellDto> MapListToDto(List<Movement> movements, List<Sell> sells)
    {
        List<SellDto> sellsDto = [];
        foreach (var sell in sells)
        {
            foreach (var movement in movements)
            {
                var sellDto = MapToDto(movement, sell);
                sellsDto.Add(sellDto);
            }
        }
        return sellsDto;
    }
}
