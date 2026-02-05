using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;

namespace MazErpBack.Services.Implementation;

public class MovementService(AppDbContext context, ILogger<MovementService> logger, MovementMapper mapper) : IMovementService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger _logger = logger;
    private readonly MovementMapper _mapper = mapper;

    public async Task<MovementDto> CreateMovementAsync(CreateMovementDto movementDto)
    {
        var user = await _context.Users.FindAsync(movementDto.UserId);
        var inventory = await _context.Inventories.FindAsync(movementDto.InventoryId);
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(inventory);

        var movement = new Movement
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
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(movement);
    }

    public async Task<bool> DeleteMovementAsync(int movementId)
    {
        throw new NotImplementedException();
    }

    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Movement>> GetMovementsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<MovementDto>> GetMovementsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MovementDto>> GetMovementsByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SoftDeleteMovementAsync(int movementID)
    {
        throw new NotImplementedException();
    }

    public async Task<MovementDto> UpdateMovementAsync(int movementId, CreateMovementDto movementDto)
    {
        throw new NotImplementedException();
    }
}
