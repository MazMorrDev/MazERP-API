using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class SellPointService(AppDbContext context, SellPointMapper mapper) : ISellPointService
{
    private readonly AppDbContext _context = context;
    private readonly SellPointMapper _mapper = mapper;

    public async Task<SellPointDto> CreateSellPointAsync(CreateSellPointDto sellPointDto)
    {
        var sellPoint = _mapper.MapDtoToModel(sellPointDto);

        await _context.AddAsync(sellPoint);
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(sellPoint);
    }

    public async Task<bool> DeleteSellPointAsync(int sellPointId)
    {
        var sellPoint = await GetSellPointByIdAsync(sellPointId);
        if (sellPoint == null)
        {
            // TODO: logging
            return false;
        }
        _context.SellPoints.Remove(sellPoint);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SellPoint> GetSellPointByIdAsync(int sellPointId)
    {
        var sellPoint = await _context.SellPoints.FindAsync(sellPointId);
        ArgumentNullException.ThrowIfNull(sellPoint);
        return sellPoint;
    }

    public async Task<List<SellPoint>> GetSellPointsAsync()
    {
        return await _context.SellPoints.ToListAsync();
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
                var sellPoint = await _context.SellPoints.FindAsync(sellPointsInventory.SellPointId);
                if (sellPoint != null) sellPointsDto.Add(_mapper.MapToDto(sellPoint));
            }
        }
        return sellPointsDto;
    }

    public async Task<bool> SoftDeleteSellPointAsync(int sellPointId)
    {
        var sellPoint = await _context.SellPoints.FindAsync(sellPointId);
        if (sellPoint == null)
        {
            //TODO: logging
            return false;
        }

        sellPoint.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SellPointDto> UpdateSellPointAsync(int sellPointId, CreateSellPointDto sellPointDto)
    {
        var sellPoint = await GetSellPointByIdAsync(sellPointId);
        sellPoint.Name = sellPointDto.Name;
        sellPoint.Description = sellPointDto.Description;
        sellPoint.Location = sellPointDto.Location;

        return _mapper.MapToDto(sellPoint);
    }
}
