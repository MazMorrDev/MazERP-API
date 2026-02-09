using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;

namespace MazErpBack.Services.Implementation;

public class BuyService(AppDbContext context, BuyMapper mapper) : IBuyService
{
    private readonly AppDbContext _context = context;
    private readonly BuyMapper _mapper = mapper;
    public async Task<BuyDto> CreateBuyAsync(CreateBuyDto createBuyDto)
    {
        var supplier = await _context.Suppliers.FindAsync(createBuyDto.SupplierId);
        var user = await _context.Users.FindAsync(createBuyDto.UserId);
        var sellPoint = await _context.SellPoints.FindAsync(createBuyDto.SellPointId);
        if (supplier == null || user == null || sellPoint == null) throw new ArgumentNullException();

        var movement = _mapper.MapMovement(user, sellPoint, createBuyDto);
        var buy = _mapper.MapBuy(movement, supplier, createBuyDto);
        await _context.Movements.AddAsync(movement);
        await _context.Buys.AddAsync(buy);
        await _context.SaveChangesAsync();

        var buyDto = _mapper.MapToDto(movement, buy);
        ArgumentNullException.ThrowIfNull(buyDto);
        return buyDto;
    }

    public async Task<bool> DeleteBuyAsync(int buyId)
    {
        throw new NotImplementedException();
    }

    public async Task<Buy> GetBuyByIdAsync(int buyId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Buy>> GetBuysAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<BuyDto>> GetBuysByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BuyDto>> GetBuysByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SoftDeleteBuyAsync(int buyID)
    {
        throw new NotImplementedException();
    }

    public async Task<BuyDto> UpdateBuyAsync(int buyId, CreateBuyDto buyDto)
    {
        throw new NotImplementedException();
    }
}
