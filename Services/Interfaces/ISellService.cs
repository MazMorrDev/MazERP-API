using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface ISellService
{
    // Only avaible for admin pannel or backend operations
    public Task<Sell> GetSellById(int sellId);
    public Task DeleteSellAsync(int sellId);

    // For common users
    public Task<List<SellDto>> GetSellsByCompanyAsync(int companyId);
    public Task<List<SellDto>> GetSellsByWarehouseAsync(int warehouseId);
    public Task<SellDto> CreateSellAsync(CreateSellDto sellDto);
    public Task<SellDto> UpdateSellAsync(int sellId, CreateSellDto sellDto);
    public Task SoftDeleteSellAsync(int sellID);
}
