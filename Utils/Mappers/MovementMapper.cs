using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class MovementMapper
{
    public MovementDto MapToDto(Movement movement)
    {
        return new MovementDto
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
    }

    public Movement MapDtoToModel(Inventory inventory, User user, CreateMovementDto movementDto)
    {
        return new Movement
        {
            UserId = movementDto.UserId,
            InventoryId = movementDto.InventoryId,
            Description = movementDto.Description,
            MovementType = movementDto.MovementType,
            Quantity = movementDto.Quantity,
            Currency = movementDto.Currency,
            MovementDate = movementDto.MovementDate,
            Inventory = inventory,
            User = user
        };
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
