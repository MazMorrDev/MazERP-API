using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpBack.Utils;

namespace MazErpAPI.Services.Interfaces;

public interface ISellPointService
{
    // Only avaible for admin pannel or backend operations
    public Task<SellPoint> GetSellPointByIdAsync(int sellPointId);
    public Task DeleteSellPointAsync(int sellPointId);

    // For common users
    public Task<PaginatedResult<SellPointDto>> GetSellPointsByCompanyAsync(int companyId, int pageNumber, int pageSize);
    public Task<SellPointInventoryDto> AssignInventoryToSellPointAsync(AssignInventoryToSellPointDto dto);
    public Task<SellPointInventoryDto> UpdateSellPointInventoryAsync(AssignInventoryToSellPointDto dto);
    public Task<PaginatedResult<SellPointDto>> GetSellPointsByWarehouseAsync(int warehouseId, int pageNumber, int pageSize);
    public Task<SellPointDto> CreateSellPointAsync(CreateSellPointDto sellPointDto);
    public Task<SellPointDto> UpdateSellPointAsync(int sellPointId, CreateSellPointDto sellPointDto);
    public Task<bool> SoftDeleteSellPointAsync(int sellPointId);
}
