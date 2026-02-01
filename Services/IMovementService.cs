using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services;

// MovementService is only for use of his childrens (Buy, Sell) following the design of the database
public interface IMovementService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Movement>> GetMovementsAsync();
    public Task<Movement> GetMovementByIdAsync(int movementId);
    public Task DeleteMovementAsync(int movementId);
    public Task<List<Movement>> GetMovementsByWorkflowAsync(int workflowId);
    public Task<List<Movement>> GetMovementsByWarehouseAsync(int warehouseId);
    public Task<Movement> CreateMovementAsync(CreateMovementDto movementDto);
    public Task<Movement> UpdateMovementAsync(UpdateMovementDto movementDto);
    public Task<Movement> SoftDeleteMovementAsync(int movementID);
}
