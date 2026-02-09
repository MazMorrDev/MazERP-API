using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class BuyService(AppDbContext context, BuyMapper mapper) : IBuyService
{
    private readonly AppDbContext _context = context;
    private readonly BuyMapper _mapper = mapper;
    public async Task<BuyDto> CreateBuyAsync(CreateBuyDto createBuyDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var supplier = await _context.Suppliers.FindAsync(createBuyDto.SupplierId);
            var user = await _context.Users.FindAsync(createBuyDto.UserId);
            var sellPoint = await _context.SellPoints.FindAsync(createBuyDto.SellPointId);
            if (supplier == null)
                throw new KeyNotFoundException($"Supplier with id {createBuyDto.SupplierId} not found");
            if (user == null)
                throw new KeyNotFoundException($"User with id {createBuyDto.UserId} not found");
            if (sellPoint == null)
                throw new KeyNotFoundException($"SellPoint with id {createBuyDto.SellPointId} not found");

            var movement = _mapper.MapMovement(user, sellPoint, createBuyDto);
            var buy = _mapper.MapBuy(movement, supplier, createBuyDto);
            await _context.Movements.AddAsync(movement);
            await _context.Buys.AddAsync(buy);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var buyDto = _mapper.MapToDto(movement, buy);
            ArgumentNullException.ThrowIfNull(buyDto);
            return buyDto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task DeleteBuyAsync(int buyId)
    {
        var movement = await GetMovementByIdAsync(buyId);
        var buy = await GetBuyByIdAsync(buyId);
        _context.Buys.Remove(buy);
        _context.Movements.Remove(movement);
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

    public async Task<List<BuyDto>> GetBuysBySellPointAsync(int sellPointId)
    {
        var movements = await _context.Movements.Where(m => m.SellPointId.Equals(sellPointId)).ToListAsync();
        List<BuyDto> buysDto = [];
        foreach (var movement in movements)
        {
            var buy = await _context.Buys.FindAsync(movement.Id);
            ArgumentNullException.ThrowIfNull(buy);
            var buyDto = _mapper.MapToDto(movement, buy);
            ArgumentNullException.ThrowIfNull(buyDto);
            buysDto.Add(buyDto);
        }
        return buysDto;
    }

    public async Task SoftDeleteBuyAsync(int movementId)
    {
        var movement = await GetMovementByIdAsync(movementId);
        movement.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<BuyDto> UpdateBuyAsync(int buyId, CreateBuyDto buyDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {

            var movement = await GetMovementByIdAsync(buyId);
            var buy = await GetBuyByIdAsync(buyId);
            if (movement == null)
                throw new KeyNotFoundException($"Movement with ID {buyId} not found");
            if (buy == null)
                throw new KeyNotFoundException($"Buy with Id {buyId} not found");

            movement.Currency = buyDto.Currency;
            movement.Description = buyDto.Description;
            movement.MovementDate = buyDto.MovementDate;
            movement.UserId = buyDto.UserId;
            movement.SellPointId = buyDto.SellPointId;
            buy.SupplierId = buyDto.SupplierId;
            buy.DeliveryStatus = buyDto.DeliveryStatus;
            buy.UnitaryCost = buyDto.UnitaryCost;
            buy.ReceivedQuantity = buyDto.ReceivedQuantity;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var buydto = _mapper.MapToDto(movement, buy);
            ArgumentNullException.ThrowIfNull(buydto);
            return buydto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
