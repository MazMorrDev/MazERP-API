using MazErpBack.DTOs.Movements;

namespace MazErpBack.Services.Implementation;

public class MovementService : IMovementService
{
    public Task<MovementDto> CreateMovementAsync(CreateMovementDto movementDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMovementAsync(int movementId)
    {
        throw new NotImplementedException();
    }

    public Task<MovementDto> GetMovementByIdAsync(int movementId)
    {
        throw new NotImplementedException();
    }

    public Task<List<MovementDto>> GetMovementsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<MovementDto>> GetMovementsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<MovementDto>> GetMovementsByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SoftDeleteMovementAsync(int movementID)
    {
        throw new NotImplementedException();
    }

    public Task<MovementDto> UpdateMovementAsync(CreateMovementDto movementDto)
    {
        throw new NotImplementedException();
    }
}
