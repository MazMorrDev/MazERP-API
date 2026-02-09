using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;

namespace MazErpBack.Services.Implementation;

public class SellService : ISellService
{
    public Task<SellDto> CreateSellAsync(CreateSellDto sellDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSellAsync(int sellId)
    {
        throw new NotImplementedException();
    }

    public Task<Sell> GetSellById(int sellId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SellDto>> GetSellsByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public Task<List<SellDto>> GetSellsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public Task SoftDeleteSellAsync(int sellID)
    {
        throw new NotImplementedException();
    }

    public Task<SellDto> UpdateSellAsync(int sellId, CreateSellDto sellDto)
    {
        throw new NotImplementedException();
    }
}
