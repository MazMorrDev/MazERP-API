using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class SellService(AppDbContext context, SellMapper mapper, ISellPointService sellPointService, IUserService userService) : ISellService
{
    private readonly AppDbContext _context = context;
    private readonly SellMapper _mapper = mapper;
    private readonly ISellPointService _sellPointService = sellPointService;
    private readonly IUserService _userService = userService;
    public async Task<SellDto> CreateSellAsync(CreateSellDto createSellDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _userService.GetUserByIdAsync(createSellDto.UserId);
            var sellPoint = await _sellPointService.GetSellPointByIdAsync(createSellDto.SellPointId);
            var movement = _mapper.MapMovement(user, createSellDto);
            var Sell = _mapper.MapSell(movement, sellPoint, createSellDto);
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
        await GetMovementByIdAsync(sellId);
        var sell = await _context.Sells.FindAsync(sellId) ?? throw new KeyNotFoundException($"Sell with id: {sellId} not found");
        return sell;
    }
    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        var movement = await _context.Movements.FindAsync(movementId);
        if (movement == null || !movement.IsActive) throw new KeyNotFoundException($"Movement with id: {movementId} not found");
        return movement;
    }

    public async Task<List<SellDto>> GetSellsBySellPointAsync(int sellPointId)
    {
        var sells = await _context.Sells.Where(m => m.SellPointId.Equals(sellPointId) && m.Movement.IsActive).ToListAsync();
        List<SellDto> SellsDto = [];
        foreach (var sell in sells)
        {
            var movement = await GetMovementByIdAsync(sell.MovementId);
            var SellDto = _mapper.MapToDto(movement, sell);
            ArgumentNullException.ThrowIfNull(SellDto);
            SellsDto.Add(SellDto);
        }
        return SellsDto;
    }

    public async Task<bool> SoftDeleteSellAsync(int movementId)
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

    public async Task<SellDto> UpdateSellAsync(int sellId, CreateSellDto sellDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {

            var movement = await GetMovementByIdAsync(sellId) ?? throw new KeyNotFoundException($"Movement with Id {sellId} not found");
            var sell = await GetSellByIdAsync(sellId) ?? throw new KeyNotFoundException($"Sell with Id {sellId} not found");

            movement.Currency = sellDto.Currency;
            movement.Description = sellDto.Description;
            movement.MovementDate = sellDto.MovementDate;
            movement.UserId = sellDto.UserId;
            sell.SellPointId = sellDto.SellPointId;
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
