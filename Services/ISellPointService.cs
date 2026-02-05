using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ISellPointService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<SellPoint>> GetSellPointsAsync();
    public Task<SellPoint> GetSellPointByIdAsync(int sellPointId);
    public Task<bool> DeleteSellPointAsync(int sellPointId);

    // For common users
    public Task<List<SellPointDto>> GetSellPointsByCompanyAsync(int companyId);
    public Task<List<SellPointDto>> GetSellPointsByWarehouseAsync(int warehouseId);
    public Task<SellPointDto> CreateSellPointAsync(CreateSellPointDto sellPointDto);
    public Task<SellPointDto> UpdateSellPointAsync(CreateSellPointDto sellPointDto);
}
