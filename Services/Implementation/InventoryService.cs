using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class InventoryService(AppDbContext context, InventoryMapper mapper) : IInventoryService
{
    private readonly AppDbContext _context = context;
    private readonly InventoryMapper _mapper = mapper;

    public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
    {
        var inventory = await _context.Inventories.FindAsync(inventoryId);
        ArgumentNullException.ThrowIfNull(inventory);
        return inventory;
    }

    public async Task<bool> SoftDeleteInventoryAsync(int inventoryId)
    {
        var inventory = await _context.Inventories.FindAsync(inventoryId);
        if (inventory == null)
        {
            //TODO: Logging
            return false;
        }
        inventory.IsActive = false;
        return true;
    }

    public async Task<InventoryDto> UpdateInventoryAsync(int inventoryId, CreateInventoryDto inventoryDto)
    {
        var inventory = await _context.Inventories.FindAsync(inventoryId);
        ArgumentNullException.ThrowIfNull(inventory);

        inventory.AlertStock = inventoryDto.AlertStock;
        inventory.WarehouseId = inventoryDto.WarehouseId;
        inventory.ProductId = inventoryDto.ProductId;
        inventory.Stock = inventoryDto.Stock;
        inventory.BasePrice = inventoryDto.BasePrice;
        inventory.BaseDiscount = inventoryDto.BaseDiscount;
        inventory.AverageCost = inventoryDto.AverageCost;
        inventory.AlertStock = inventoryDto.AlertStock;
        inventory.AlertStock = inventoryDto.AlertStock;

        return _mapper.MapToDto(inventory);
    }

    public async Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto inventoryDto)
    {
        try
        {
            var warehouse = await _context.Warehouses.FindAsync(inventoryDto.WarehouseId);
            var product = await _context.Products.FindAsync(inventoryDto.ProductId);


            if (warehouse == null || product == null)
            {
                //TODO: logging
                throw new ArgumentException("Almacén o Producto no encontrados");
            }

            var inventory = _mapper.MapDtoToModel(warehouse, product, inventoryDto);

            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();

            return _mapper.MapToDto(inventory);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteInventoryAsync(int inventoryId)
    {
        try
        {
            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory == null)
            {
                // poner el logger
                return false;
            }

            inventory.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<InventoryDto>> GetInventoriesByWarehouseAsync(int warehouseId)
    {
        try
        {
            var inventories = await _context.Inventories.Where(i => i.WarehouseId.Equals(warehouseId)).ToListAsync();
            return _mapper.MapListToDto(inventories);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<InventoryDto>> GetInventoriesByCompanyAsync(int companyId)
    {
        var warehouses = await _context.Warehouses.Where(c => c.CompanyId.Equals(companyId)).ToListAsync();
        List<InventoryDto> inventoriesDto = [];
        foreach (var warehouse in warehouses)
        {
            inventoriesDto.AddRange(await GetInventoriesByWarehouseAsync(warehouse.Id));
        }
        return inventoriesDto;
    }
}
