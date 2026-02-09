using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class SellMapper(ILogger<SellMapper> logger)
{
    private readonly ILogger<SellMapper> _logger = logger;

    public SellDto? MapToDto(Movement movement, Sell Sell)
    {
        if (movement.Id != Sell.MovementId)
        {
            _logger.LogError("se está intentando mapear un Sell con un id que no es el mismo del Movement");
            return null;
        }

        return new SellDto
        {
            MovementId = movement.Id,
            UserId = movement.UserId,
            SellPointId = movement.SellPointId,
            Description = movement.Description,
            Quantity = movement.Quantity,
            Currency = movement.Currency,
            MovementDate = movement.MovementDate,
            DiscountPercentage = Sell.DiscountPercentage,
            PaymentStatus = Sell.PaymentStatus,
            SaleType = Sell.SaleType,
            SellerNotes = Sell.SellerNotes
        };
    }

    public Sell MapSell(Movement movement, CreateSellDto SellDto)
    {
        return new Sell
        {
            MovementId = movement.Id,
            DiscountPercentage = SellDto.DiscountPercentage,
            PaymentStatus = SellDto.PaymentStatus,
            SaleType = SellDto.SaleType,
            SellerNotes = SellDto.SellerNotes,
            Movement = movement
        };
    }

    public Movement MapMovement(User user, SellPoint sellPoint, CreateSellDto SellDto)
    {
        return new Movement
        {
            UserId = SellDto.UserId,
            SellPointId = SellDto.SellPointId,
            Description = SellDto.Description,
            Quantity = SellDto.Quantity,
            Currency = SellDto.Currency,
            MovementDate = SellDto.MovementDate,
            SellPoint = sellPoint,
            User = user
        };
    }

    public List<SellDto>? MapListToDto(List<Movement> movements, List<Sell> sells)
    {
        List<SellDto> sellsDto = [];
        foreach (var sell in sells)
        {
            foreach (var movement in movements)
            {
                var sellDto = MapToDto(movement, sell);
                if (sellDto == null) return null;
                sellsDto.Add(sellDto);
            }
        }
        return sellsDto;
    }
}
