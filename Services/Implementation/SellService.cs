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
        var user = await _context.Users.FindAsync(createSellDto.UserId);
        var sellPoint = await _context.SellPoints.FindAsync(createSellDto.SellPointId);
        if (user == null || sellPoint == null) throw new ArgumentNullException();

        var movement = _mapper.MapMovement(user, sellPoint, createSellDto);
        var Sell = _mapper.MapSell(movement, createSellDto);
        await _context.Movements.AddAsync(movement);
        await _context.Sells.AddAsync(Sell);
        await _context.SaveChangesAsync();

        var SellDto = _mapper.MapToDto(movement, Sell);
        ArgumentNullException.ThrowIfNull(SellDto);
        return SellDto;
    }

    public async Task DeleteSellAsync(int SellId)
    {
        var Sell = await GetSellByIdAsync(SellId);
        _context.Sells.Remove(Sell);
        await _context.SaveChangesAsync();
    }

    public async Task<Sell> GetSellByIdAsync(int SellId)
    {
        var Sell = await _context.Sells.FindAsync(SellId);
        ArgumentNullException.ThrowIfNull(Sell);
        return Sell;
    }
    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        var movement = await _context.Movements.FindAsync(movementId);
        ArgumentNullException.ThrowIfNull(movement);
        return movement;
    }

    public async Task<List<SellDto>> GetSellsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
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
        // TODO: Tengo mis dudas de si esto updatea en la base de datos
        var movement = await GetMovementByIdAsync(movementId);
        movement.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<SellDto> UpdateSellAsync(int SellId, CreateSellDto SellDto)
    {
        var movement = await GetMovementByIdAsync(SellId);
        var Sell = await GetSellByIdAsync(SellId);
        if (movement == null || Sell == null) throw new ArgumentNullException();

        movement.Currency = SellDto.Currency;
        movement.Description = SellDto.Description;
        movement.MovementDate = SellDto.MovementDate;
        movement.UserId = SellDto.UserId;
        movement.SellPointId = SellDto.SellPointId;
        Sell

        var Selldto = _mapper.MapToDto(movement, Sell);
        ArgumentNullException.ThrowIfNull(Selldto);
        return Selldto;
    }
}
