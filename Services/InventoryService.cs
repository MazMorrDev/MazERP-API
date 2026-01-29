using MazErpBack.Context;
using MazErpBack.Dtos.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services;

public class InventoryService(AppDbContext context) : IInventoryService
{
    private readonly AppDbContext _context = context;

    public async Task<Inventory> CreateInventoryAsync(CreateInventoryDto inventoryDto)
    {
        try
        {
            var inventory = new Inventory()
            {
                WarehouseId = inventoryDto.WarehouseId,
                ProductId = inventoryDto.ProductId,
                Stock = inventoryDto.Stock,
                AlertStock = inventoryDto.AlertStock,
                WarningStock = inventoryDto.WarningStock,
            };

            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();

            return inventory;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Inventory> DeleteInventoryAsync(int id)
    {
        try
        {
            var inventory = await _context.Inventories.FindAsync(id);
            ArgumentNullException.ThrowIfNull(inventory);

            inventory.IsActive = false;
            await _context.SaveChangesAsync();

            return inventory;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Inventory>> GetInventoriesByWarehouseAsync(int id)
    {
        try
        {
            var inventories = await _context.Inventories.Where(w => w.WarehouseId.Equals(id)).ToListAsync();
            return inventories;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
