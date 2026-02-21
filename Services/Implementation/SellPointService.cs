using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

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

    public async Task<List<SellPointDto>> GetSellPointsByCompanyAsync(int companyId)
    {
        var warehouses = await _context.Warehouses.Where(w => w.CompanyId == companyId).ToListAsync();
        List<SellPointDto> sellPointsDto = [];
        foreach (var warehouse in warehouses)
        {
            sellPointsDto.AddRange(await GetSellPointsByWarehouseAsync(warehouse.Id));
        }
        return sellPointsDto;
    }

    public async Task<List<SellPointDto>> GetSellPointsByWarehouseAsync(int warehouseId)
    {
        var inventories = await _context.Inventories.Where(i => i.WarehouseId == warehouseId).ToListAsync();
        List<SellPointDto> sellPointsDto = [];
        foreach (var inventory in inventories)
        {
            // TODO: ver bien como se hace esta consulta teniendo en cuenta que hay 2 keys
            var sellPointInventories = await _context.SellPointInventories.Where(spi => spi.InventoryId == inventory.Id).ToListAsync();
            foreach (var sellPointsInventory in sellPointInventories)
            {
                var sellPoint = await GetSellPointByIdAsync(sellPointsInventory.SellPointId);
                if (sellPoint != null) sellPointsDto.Add(_mapper.MapToDto(sellPoint));
            }
        }
        return sellPointsDto;
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
