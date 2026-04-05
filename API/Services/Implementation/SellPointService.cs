using MazErpAPI.Context;
using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpAPI.Services.Implementation;

public class SellPointService(AppDbContext context, SellPointMapper mapper, IInventoryService inventoryService) : ISellPointService
{
    private readonly AppDbContext _context = context;
    private readonly IInventoryService _inventoryService = inventoryService;
    private readonly SellPointMapper _mapper = mapper;

    public async Task<SellPointInventoryDto> AssignInventoryToSellPointAsync(AssignInventoryToSellPointDto dto)
    {
        // Verificar que el inventario existe
        var inventory = await _inventoryService.GetInventoryByIdAsync(dto.InventoryId);

        // Verificar que el punto de venta existe
        var sellPoint = await GetSellPointByIdAsync(dto.SellPointId);

        // Verificar que NO exista ya la asignación
        var existingAssignment = await _context.SellPointInventories
            .FirstOrDefaultAsync(spi => spi.InventoryId == dto.InventoryId && spi.SellPointId == dto.SellPointId);

        if (existingAssignment != null)
        {
            return await UpdateSellPointInventoryAsync(dto);
        }

        // Crear nueva asignación
        var sellPointInventory = _mapper.MapSellPointInventory(dto, inventory, sellPoint);
        await _context.SellPointInventories.AddAsync(sellPointInventory);
        await _context.SaveChangesAsync();

        return _mapper.MapSellPointInventoryDto(sellPointInventory);
    }

    public async Task<SellPointInventoryDto> UpdateSellPointInventoryAsync(AssignInventoryToSellPointDto dto)
    {
        // Verificar que el inventario existe
        var inventory = await _inventoryService.GetInventoryByIdAsync(dto.InventoryId);

        // Verificar que el punto de venta existe
        var sellPoint = await GetSellPointByIdAsync(dto.SellPointId);

        // Buscar la asignación existente
        var existingAssignment = await _context.SellPointInventories
            .FirstOrDefaultAsync(spi => spi.InventoryId == dto.InventoryId && spi.SellPointId == dto.SellPointId);

        if (existingAssignment == null)
        {
            return await AssignInventoryToSellPointAsync(dto);
        }

        // Actualizar la asignación existente
        existingAssignment.SellPrice = dto.SellPrice;
        existingAssignment.Discount = dto.Discount;
        existingAssignment.Stock = dto.Stock;
        existingAssignment.WarningStock = dto.WarningStock;
        existingAssignment.AlertStock = dto.AlertStock;

        _context.SellPointInventories.Update(existingAssignment);
        await _context.SaveChangesAsync();

        return _mapper.MapSellPointInventoryDto(existingAssignment);
    }


    public async Task<SellPointDto> CreateSellPointAsync(CreateSellPointDto sellPointDto)
    {
        var sellPoint = _mapper.MapDtoToModel(sellPointDto);

        await _context.AddAsync(sellPoint);
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(sellPoint);
    }

    public async Task DeleteSellPointAsync(int sellPointId)
    {
        var sellPoint = await GetSellPointByIdAsync(sellPointId);
        _context.SellPoints.Remove(sellPoint);
        await _context.SaveChangesAsync();
    }

    public async Task<SellPoint> GetSellPointByIdAsync(int sellPointId)
    {
        var sellPoint = await _context.SellPoints.FindAsync(sellPointId);
        if (sellPoint == null || !sellPoint.IsActive) throw new KeyNotFoundException($"SellPoint with id: {sellPointId} not found");
        return sellPoint;
    }

    public async Task<PaginatedResult<SellPointDto>> GetSellPointsByCompanyAsync(int companyId, int pageNumber, int pageSize)
    {
        var query = _context.Inventories
            .Where(i => i.Warehouse.CompanyId == companyId && i.IsActive)
            .SelectMany(i => i.SellPointInventories)
            .Select(spi => spi.SellPoint)
            .Distinct();

        var totalCount = await query.CountAsync();

        var sellPoints = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var sellPointsDto = sellPoints.Select(_mapper.MapToDto).ToList();

        return new PaginatedResult<SellPointDto>(sellPointsDto, totalCount, pageNumber, pageSize);
    }
    
    public async Task<PaginatedResult<SellPointDto>> GetSellPointsByWarehouseAsync(int warehouseId, int pageNumber, int pageSize)
    {
        var query = _context.Inventories
            .Where(i => i.WarehouseId == warehouseId && i.IsActive)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var totalCount = await _context.Inventories
            .Where(i => i.WarehouseId == warehouseId && i.IsActive)
            .CountAsync();

        var sellPointsDto = await query
            .SelectMany(inventory => inventory.SellPointInventories)
            .Select(spi => spi.SellPoint)
            .Where(sellPoint => sellPoint != null)
            .Distinct() // Optional: remove duplicates if an inventory can have the same sell point
            .Select(sellPoint => _mapper.MapToDto(sellPoint))
            .ToListAsync();

        return new PaginatedResult<SellPointDto>(sellPointsDto, totalCount, pageNumber, pageSize);
    }

    public async Task<bool> SoftDeleteSellPointAsync(int sellPointId)
    {
        try
        {
            var sellPoint = await GetSellPointByIdAsync(sellPointId);
            sellPoint.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }

    }

    public async Task<SellPointDto> UpdateSellPointAsync(int sellPointId, CreateSellPointDto sellPointDto)
    {
        var sellPoint = await GetSellPointByIdAsync(sellPointId);
        sellPoint.Name = sellPointDto.Name;
        sellPoint.Description = sellPointDto.Description;
        sellPoint.Location = sellPointDto.Location;

        await _context.SaveChangesAsync();
        return _mapper.MapToDto(sellPoint);
    }
}
