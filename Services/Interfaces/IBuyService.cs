using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Utils;

namespace MazErpBack.Services.Interfaces;

public interface IBuyService
{
    // Only avaible for admin pannel or backend operations
    public Task<Movement> GetMovementByIdAsync(int movementId);
    public Task<Buy> GetBuyByIdAsync(int buyId);
    public Task DeleteBuyAsync(int buyId);

    // For common users
    public Task<PaginatedResult<BuyDto>> GetBuysByInventoryAsync(int invId, int pageNumber, int pageSize);
    public Task<BuyDto> CreateBuyAsync(CreateBuyDto buyDto);
    public Task<BuyDto> UpdateBuyAsync(int buyId, CreateBuyDto buyDto);
    public Task<bool> SoftDeleteBuyAsync(int buyID);
}
