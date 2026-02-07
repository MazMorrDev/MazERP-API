using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

// MovementService is only for use of his childrens (Buy, Sell) following the design of the database
public interface IMovementService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Movement>> GetMovementsAsync();
    public Task<Movement> GetMovementByIdAsync(int movementId);
    public Task<bool> DeleteMovementAsync(int movementId);

    // for users
    public Task<List<MovementDto>> GetMovementsByCompanyAsync(int companyId);
    public Task<List<MovementDto>> GetMovementsByWarehouseAsync(int warehouseId);
    public Task<List<MovementDto>> GetMovementsBySellPointAsync(int sellPoint);
    public Task<MovementDto> CreateMovementAsync(CreateMovementDto movementDto);
    public Task<MovementDto> UpdateMovementAsync(int movementId, CreateMovementDto movementDto);
    public Task<bool> SoftDeleteMovementAsync(int movementID);
}
