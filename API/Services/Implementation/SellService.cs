using MazErpAPI.Context;
using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MazErpAPI.Services.Implementation;

public class SellService(AppDbContext context, SellMapper mapper, ISellPointService sellPointService, IUserService userService, ILogger<SellService> logger) : ISellService
{
    private readonly AppDbContext _context = context;
    private readonly SellMapper _mapper = mapper;
    private readonly ISellPointService _sellPointService = sellPointService;
    private readonly IUserService _userService = userService;
    private readonly ILogger<SellService> _logger = logger;

    public async Task<SellDto> CreateSellAsync(CreateSellDto createSellDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _userService.GetUserByIdAsync(createSellDto.UserId);
            var sellPoint = await _sellPointService.GetSellPointByIdAsync(createSellDto.SellPointId);
            var movement = _mapper.MapMovement(user, createSellDto);
            var sell = _mapper.MapSell(movement, sellPoint, createSellDto);
            await _context.Movements.AddAsync(movement);
            await _context.Sells.AddAsync(sell);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var sellDto = _mapper.MapToDto(movement, sell);
            ArgumentNullException.ThrowIfNull(sellDto);
            return sellDto;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating sell");
            throw;
        }
    }

    public async Task DeleteSellAsync(int sellId)
    {
        try
        {
            var movement = await GetMovementByIdAsync(sellId);
            var sell = await GetSellByIdAsync(sellId);
            _context.Sells.Remove(sell);
            _context.Movements.Remove(movement);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Sell {SellId} deleted successfully", sellId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sell {SellId}", sellId);
            throw;
        }
    }

    public async Task<Sell> GetSellByIdAsync(int sellId)
    {
        try
        {
            await GetMovementByIdAsync(sellId);
            var sell = await _context.Sells.FindAsync(sellId) ?? throw new KeyNotFoundException($"Sell with id: {sellId} not found");
            return sell;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sell by id {SellId}", sellId);
            throw;
        }
    }
    
    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        try
        {
            var movement = await _context.Movements.FindAsync(movementId);
            if (movement == null || !movement.IsActive) throw new KeyNotFoundException($"Movement with id: {movementId} not found");
            return movement;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting movement by id {MovementId}", movementId);
            throw;
        }
    }

    public async Task<PaginatedResult<SellDto>> GetSellsBySellPointAsync(int sellPointId, int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Sells.Include(m => m.Movement).Where(m => m.SellPointId == sellPointId && m.Movement.IsActive);
            var totalCount = await query.CountAsync();
            var sells = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var sellsDto = _mapper.MapListToDto([.. sells.Select(s => s.Movement)], sells);

            return new PaginatedResult<SellDto>(sellsDto, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sells by sell point {SellPointId}", sellPointId);
            throw;
        }
    }

    public async Task<bool> SoftDeleteSellAsync(int movementId)
    {
        try
        {
            var movement = await GetMovementByIdAsync(movementId);
            movement.IsActive = false;
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Sell {MovementId} soft deleted successfully", movementId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error soft deleting sell {MovementId}", movementId);
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

            var sellDtoResult = _mapper.MapToDto(movement, sell);
            ArgumentNullException.ThrowIfNull(sellDtoResult);
            return sellDtoResult;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error updating sell {SellId}", sellId);
            throw;
        }
    }
}