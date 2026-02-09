using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;

namespace MazErpBack.Services.Implementation;

public class SellService(AppDbContext context, SellMapper mapper) : ISellService
{
    private readonly AppDbContext _context = context;
    private readonly SellMapper _mapper = mapper;

    public async Task<SellDto> CreateSellAsync(CreateSellDto sellDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteSellAsync(int sellId)
    {
        var sell = await GetSellByIdAsync(sellId);
    }

    public async Task<Sell> GetSellByIdAsync(int sellId)
    {
        var sell = await _context.Sells.FindAsync(sellId);
    }

    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        var movementId
    }

    public async Task<List<SellDto>> GetSellsByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SellDto>> GetSellsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task SoftDeleteSellAsync(int sellID)
    {
        throw new NotImplementedException();
    }

    public Task<SellDto> UpdateSellAsync(int sellId, CreateSellDto sellDto)
    {
        throw new NotImplementedException();
    }
}
