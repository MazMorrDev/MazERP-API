using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;

namespace MazErpBack.Services.Implementation;

public class MovementService(AppDbContext context, ILogger<MovementService> logger, MovementMapper mapper) : IMovementService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger _logger = logger;
    private readonly MovementMapper _mapper = mapper;

    public async Task<MovementDto> CreateMovementAsync(CreateMovementDto movementDto)
    {
        var movement = new Movement
        {

        };
        return _mapper.movement;
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
