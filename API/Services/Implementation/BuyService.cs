using MazErpAPI.Context;
using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpBack.Utils;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpAPI.Services.Implementation;

public class BuyService(AppDbContext context, BuyMapper mapper, IUserService userService, ISupplierService supplierService, IInventoryService inventoryService) : IBuyService
{
    private readonly AppDbContext _context = context;
    private readonly BuyMapper _mapper = mapper;
    private readonly IUserService _userService = userService;
    private readonly ISupplierService _supplierService = supplierService;
    private readonly IInventoryService _inventoryService = inventoryService;

    public async Task<BuyDto> CreateBuyAsync(CreateBuyDto createBuyDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(createBuyDto.SupplierId);
            var user = await _userService.GetUserByIdAsync(createBuyDto.UserId);
            var inventory = await _inventoryService.GetInventoryByIdAsync(createBuyDto.InventoryId);

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
        await GetMovementByIdAsync(buyId);
        var buy = await _context.Buys.FindAsync(buyId) ?? throw new KeyNotFoundException($"Buy with id: {buyId} not found");
        return buy;
    }
    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        var movement = await _context.Movements.FindAsync(movementId);
        if (movement == null || !movement.IsActive) throw new KeyNotFoundException($"Movement with id: {movementId} not found");
        return movement;
    }

    public async Task<PaginatedResult<BuyDto>> GetBuysByInventoryAsync(int invId, int pageNumber, int pageSize)
    {
        // 1. Consulta optimizada con Include (una sola consulta a BD)
        var query = _context.Buys.Include(b => b.Movement)  // <-- ESTO ES LA CLAVE
            .Where(m => m.InventoryId == invId && m.Movement.IsActive);

        // 2. Total de registros (rápido, solo COUNT)
        var totalCount = await query.CountAsync();

        // 3. Paginación en BD y obtención de datos
        var buys = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();

        // 4. Mapeo directo (sin bucles manuales)
        var buyDtos = _mapper.MapListToDto(buys.Select(b => b.Movement).ToList(), buys);

        return new PaginatedResult<BuyDto>(buyDtos, totalCount, pageNumber, pageSize);
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
