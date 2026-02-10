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
            var inventory = await _context.Inventories.FindAsync(createBuyDto.InventoryId);
            if (supplier == null)
                throw new KeyNotFoundException($"Supplier with id {createBuyDto.SupplierId} not found");
            if (user == null)
                throw new KeyNotFoundException($"User with id {createBuyDto.UserId} not found");
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory with id {createBuyDto.InventoryId} not found");

            var movement = _mapper.MapMovement(user, createBuyDto);
            var buy = _mapper.MapBuy(movement, supplier, inventory, createBuyDto);
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

    public async Task<List<BuyDto>> GetBuysByInventoryAsync(int invId)
    {
        var buys = await _context.Buys.Where(m => m.InventoryId.Equals(invId)).ToListAsync();
        List<Movement> movements = [];
        foreach (var b in buys)
        {
            var m = await _context.Movements.FindAsync(b.MovementId) ?? throw new KeyNotFoundException($"Movement with id: {b.MovementId} not found");
            movements.Add(m);
        }
        return _mapper.MapListToDto(movements, buys);
    }

    public async Task<bool> SoftDeleteBuyAsync(int movementId)
    {
        try
        {
            var movement = await GetMovementByIdAsync(movementId);
            movement.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }

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
