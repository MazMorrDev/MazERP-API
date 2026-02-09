using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface ISellService
{
    // Only avaible for admin pannel or backend operations
    public Task<Sell> GetSellByIdAsync(int sellId);
    public Task DeleteSellAsync(int sellId);

    // For common users
    public Task<List<SellDto>> GetSellsBySellPointAsync(int sellPointId);
    public Task<List<SellDto>> GetSellsByWarehouseAsync(int warehouseId);
    public Task<SellDto> CreateSellAsync(CreateSellDto sellDto);
    public Task<SellDto> UpdateSellAsync(int sellId, CreateSellDto sellDto);
    public Task SoftDeleteSellAsync(int sellID);
}
