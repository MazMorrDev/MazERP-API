using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services;

// MovementService is only for use of his childrens (Buy, Sell) following the design of the database
public interface IMovementService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Movement>> GetMovementsAsync();
    public Task<Movement> GetMovementByIdAsync(int movementId);
    public Task<bool> DeleteMovementAsync(int movementId);

    // for users
    public Task<List<MovementDto>> GetMovementsByWorkflowAsync(int workflowId);
    public Task<List<MovementDto>> GetMovementsByWarehouseAsync(int warehouseId);
    public Task<MovementDto> CreateMovementAsync(CreateMovementDto movementDto);
    public Task<MovementDto> UpdateMovementAsync(CreateMovementDto movementDto);
    public Task<bool> SoftDeleteMovementAsync(int movementID);
}
