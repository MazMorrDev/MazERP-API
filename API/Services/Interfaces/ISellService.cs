using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpAPI.Utils;

namespace MazErpAPI.Services.Interfaces;

public interface ISellService
{
    // Only avaible for admin pannel or backend operations
    public Task<Sell> GetSellByIdAsync(int sellId);
    public Task DeleteSellAsync(int sellId);

    // For common users
    public Task<PaginatedResult<SellDto>> GetSellsBySellPointAsync(int sellPointId, int pageNumber, int pageSize);
    public Task<SellDto> CreateSellAsync(CreateSellDto sellDto);
    public Task<SellDto> UpdateSellAsync(int sellId, CreateSellDto sellDto);
    public Task<bool> SoftDeleteSellAsync(int sellID);
}
