using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class MovementMapper
{
    public MovementDto MapToDto(Movement movement)
    {
        var movementDto = new MovementDto
        {
            MovementId = movement.Id,
            UserId = movement.UserId,
            InventoryId = movement.InventoryId,
            Description = movement.Description,
            MovementType = movement.MovementType,
            Quantity = movement.Quantity,
            Currency = movement.Currency,
            MovementDate = movement.MovementDate
        };
        return movementDto;
    }

    public List<MovementDto> MapListToDto(List<Movement> movements)
    {
        List<MovementDto> movementsDto = [];
        foreach (var movement in movements)
        {
            movementsDto.Add(MapToDto(movement));
        }
        return movementsDto;
    }
}
