using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class SellService(AppDbContext context, SellMapper mapper) : ISellService
{
    private readonly AppDbContext _context = context;
    private readonly SellMapper _mapper = mapper;
    public async Task<SellDto> CreateSellAsync(CreateSellDto createSellDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _context.Users.FindAsync(createSellDto.UserId);
            var sellPoint = await _context.SellPoints.FindAsync(createSellDto.SellPointId);
            if (user == null)
                throw new KeyNotFoundException($"User with id {createSellDto.UserId} not found");
            if (sellPoint == null)
                throw new KeyNotFoundException($"SellPoint with id {createSellDto.SellPointId} not found");

            var movement = _mapper.MapMovement(user, sellPoint, createSellDto);
            var Sell = _mapper.MapSell(movement, createSellDto);
            await _context.Movements.AddAsync(movement);
            await _context.Sells.AddAsync(Sell);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var SellDto = _mapper.MapToDto(movement, Sell);
            ArgumentNullException.ThrowIfNull(SellDto);
            return SellDto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task DeleteSellAsync(int sellId)
    {
        var movement = await GetMovementByIdAsync(sellId);
        var sell = await GetSellByIdAsync(sellId);
        _context.Sells.Remove(sell);
        _context.Movements.Remove(movement);
        await _context.SaveChangesAsync();
    }

    public async Task<Sell> GetSellByIdAsync(int sellId)
    {
        var sell = await _context.Sells.FindAsync(sellId);
        ArgumentNullException.ThrowIfNull(sell);
        return sell;
    }
    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        var movement = await _context.Movements.FindAsync(movementId);
        ArgumentNullException.ThrowIfNull(movement);
        return movement;
    }

    public async Task<List<SellDto>> GetSellsBySellPointAsync(int sellPointId)
    {
        var movements = await _context.Movements.Where(m => m.SellPointId.Equals(sellPointId)).ToListAsync();
        List<SellDto> SellsDto = [];
        foreach (var movement in movements)
        {
            var Sell = await _context.Sells.FindAsync(movement.Id);
            ArgumentNullException.ThrowIfNull(Sell);
            var SellDto = _mapper.MapToDto(movement, Sell);
            ArgumentNullException.ThrowIfNull(SellDto);
            SellsDto.Add(SellDto);
        }
        return SellsDto;
    }

    public async Task SoftDeleteSellAsync(int movementId)
    {
        var movement = await GetMovementByIdAsync(movementId);
        movement.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<SellDto> UpdateSellAsync(int sellId, CreateSellDto sellDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {

            var movement = await GetMovementByIdAsync(sellId);
            var sell = await GetSellByIdAsync(sellId);
            if (movement == null)
                throw new KeyNotFoundException($"Movement with ID {sellId} not found");
            if (sell == null)
                throw new KeyNotFoundException($"Sell with Id {sellId} not found");

            movement.Currency = sellDto.Currency;
            movement.Description = sellDto.Description;
            movement.MovementDate = sellDto.MovementDate;
            movement.UserId = sellDto.UserId;
            movement.SellPointId = sellDto.SellPointId;
            sell.DiscountPercentage = sellDto.DiscountPercentage;
            sell.PaymentStatus = sellDto.PaymentStatus;
            sell.SaleType = sellDto.SaleType;
            sell.SellerNotes = sellDto.SellerNotes;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var Selldto = _mapper.MapToDto(movement, sell);
            ArgumentNullException.ThrowIfNull(Selldto);
            return Selldto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
