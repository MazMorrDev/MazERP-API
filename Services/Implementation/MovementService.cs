using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public partial class MovementService(AppDbContext context, ILogger<MovementService> logger, MovementMapper mapper) : IMovementService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<MovementService> _logger = logger;
    private readonly MovementMapper _mapper = mapper;

    // DECLARACIONES DE MÉTODOS PARTIAL PARA LOGGING
    [LoggerMessage(Level = LogLevel.Warning, Message = "Movement {MovementId} no encontrado")]
    private partial void LogMovementNotFoundWarning(int movementId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "No se puede eliminar un movimiento asociado a un inventory con id:{inventoryId} inactivo")]
    private partial void LogInventoryNotActiveWarning(int inventoryId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Movimiento {MovementId} creado exitosamente")]
    private partial void LogMovementCreated(int movementId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Error al crear movimiento. Usuario {UserId} o Inventory {InventoryId} no encontrados")]
    private partial void LogMovementCreationError(int userId, int inventoryId);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Obteniendo movimiento {MovementId}")]
    private partial void LogGettingMovement(int movementId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Movimiento {MovementId} eliminado físicamente")]
    private partial void LogMovementHardDeleted(int movementId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Movimiento {MovementId} eliminado lógicamente (soft delete)")]
    private partial void LogMovementSoftDeleted(int movementId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Intento de editar movimiento antiguo {MovementId}. Fecha: {MovementDate}")]
    private partial void LogAttemptEditOldMovement(int movementId, DateTimeOffset movementDate);

    public async Task<MovementDto> CreateMovementAsync(CreateMovementDto movementDto)
    {
        var user = await _context.Users.FindAsync(movementDto.UserId);
        var inventory = await _context.Inventories.FindAsync(movementDto.SellPointId);

        if (user == null || inventory == null)
        {
            LogMovementCreationError(movementDto.UserId, movementDto.SellPointId);
            throw new ArgumentException("Usuario o SellPoint no encontrados");
        }

        var movement = _mapper.MapDtoToModel(inventory, user, movementDto);

        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        LogMovementCreated(movement.Id);
        return _mapper.MapToDto(movement);
    }

    public async Task<bool> DeleteMovementAsync(int movementId)
    {
        var movement = await _context.Movements
            .Include(m => m.SellPoint)
            .FirstOrDefaultAsync(m => m.Id == movementId);

        if (movement == null)
        {
            LogMovementNotFoundWarning(movementId);
            return false;
        }

        // CORRECCIÓN: La condición estaba invertida. Debería ser !IsActive
        if (movement.SellPoint != null && !movement.SellPoint.IsActive)
        {
            LogInventoryNotActiveWarning(movement.SellPoint.Id);
            return false;
        }

        _context.Movements.Remove(movement);
        await _context.SaveChangesAsync();

        LogMovementHardDeleted(movementId);
        return true;
    }

    public async Task<Movement> GetMovementByIdAsync(int movementId)
    {
        LogGettingMovement(movementId);

        var movement = await _context.Movements.FindAsync(movementId);

        if (movement == null)
        {
            LogMovementNotFoundWarning(movementId);
            throw new KeyNotFoundException($"Movement {movementId} not found");
        }

        return movement;
    }

    public async Task<List<Movement>> GetMovementsAsync()
    {
        var movements = await _context.Movements.ToListAsync();

        if (movements == null || movements.Count == 0)
        {
            _logger.LogDebug("No se encontraron movimientos");
        }
        else
        {
            _logger.LogDebug("Se obtuvieron {Count} movimientos", movements.Count);
        }

        return movements ?? [];
    }

    public async Task<bool> SoftDeleteMovementAsync(int movementID)
    {
        var movement = await _context.Movements.FindAsync(movementID);

        if (movement == null)
        {
            LogMovementNotFoundWarning(movementID);
            return false;
        }

        movement.IsActive = false;
        movement.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        LogMovementSoftDeleted(movementID);
        return true;
    }

    public async Task<MovementDto> UpdateMovementAsync(int movementId, CreateMovementDto movementDto)
    {
        var movement = await _context.Movements
            .Include(m => m.SellPoint)
            .FirstOrDefaultAsync(m => m.Id == movementId);

        if (movement == null)
        {
            LogMovementNotFoundWarning(movementId);
            throw new KeyNotFoundException($"Movement {movementId} not found");
        }

        // Verificar si el movimiento es editable
        if (movement.MovementDate < DateTime.UtcNow.AddDays(-1))
        {
            LogAttemptEditOldMovement(movementId, movement.MovementDate);
            throw new InvalidOperationException("No se pueden editar movimientos antiguos");
        }

        movement.UserId = movementDto.UserId;
        movement.SellPointId = movementDto.SellPointId;
        movement.Description = movementDto.Description;
        movement.MovementType = movementDto.MovementType;
        movement.Quantity = movementDto.Quantity;
        movement.Currency = movementDto.Currency;
        movement.MovementDate = movementDto.MovementDate;

        await _context.SaveChangesAsync();

        return _mapper.MapToDto(movement);
    }

    // Métodos no implementados
    public async Task<List<MovementDto>> GetMovementsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MovementDto>> GetMovementsByInventoryAsync(int InventoryId)
    {
        throw new NotImplementedException();
    }

    public Task<List<MovementDto>> GetMovementsByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public Task<List<MovementDto>> GetMovementsBySellPointAsync(int sellPoint)
    {
        throw new NotImplementedException();
    }
}