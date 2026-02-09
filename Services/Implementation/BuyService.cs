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

    public async Task DeleteBuyAsync(int buyId)
    {
        var buy = await GetBuyByIdAsync(buyId);
        _context.Buys.Remove(buy);
        await _context.SaveChangesAsync();
    }

    public async Task<Buy> GetBuyByIdAsync(int buyId)
    {
        var buy = await _context.Buys.FindAsync(buyId);
        ArgumentNullException.ThrowIfNull(buy);
        return buy;
    }
    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        var movement = await _context.Movements.FindAsync(movementId);
        ArgumentNullException.ThrowIfNull(movement);
        return movement;
    }

    public async Task<List<BuyDto>> GetBuysByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BuyDto>> GetBuysByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public async Task SoftDeleteBuyAsync(int movementId)
    {
        // TODO: Tengo mis dudas de si esto updatea en la base de datos
        var movement = await GetMovementByIdAsync(movementId);
        movement.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<BuyDto> UpdateBuyAsync(int buyId, CreateBuyDto buyDto)
    {
        var movement = await GetMovementByIdAsync(buyId);
        var buy = await GetBuyByIdAsync(buyId);
        if (movement == null || buy == null) throw new ArgumentNullException();

        movement.Currency = buyDto.Currency;
        movement.Description = buyDto.Description;
        movement.MovementDate = buyDto.MovementDate;
        movement.UserId = buyDto.UserId;
        movement.SellPointId = buyDto.SellPointId;

        return _mapper.MapToDto(movement, buy);
    }
}
