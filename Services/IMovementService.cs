using MazErpBack.DTOs.Movements;

namespace MazErpBack.Services;

// MovementService is only for use of his childrens (Buy, Sell) following the design of the database
public interface IMovementService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<MovementDto>> GetMovementsAsync();
    public Task<MovementDto> GetMovementByIdAsync(int movementId);
    public Task<bool> DeleteMovementAsync(int movementId);
    public Task<List<MovementDto>> GetMovementsByWorkflowAsync(int workflowId);
    public Task<List<MovementDto>> GetMovementsByWarehouseAsync(int warehouseId);
    public Task<MovementDto> CreateMovementAsync(CreateMovementDto movementDto);
    public Task<MovementDto> UpdateMovementAsync(CreateMovementDto movementDto);
    public Task<bool> SoftDeleteMovementAsync(int movementID);
}
