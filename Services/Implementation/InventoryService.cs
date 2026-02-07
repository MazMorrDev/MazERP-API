using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class InventoryService(AppDbContext context) : IInventoryService
{
    private readonly AppDbContext _context = context;

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

    public Task<List<Inventory>> GetInventoriesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Inventory> GetInventoryByIdAsync()
    {
        var inventory = await
        return
    }

    public async Task<bool> SoftDeleteInventoryAsync(int inventoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<InventoryDto> UpdateInventoryAsync(int inventoryId, CreateInventoryDto inventoryDto)
    {
        throw new NotImplementedException();
    }

    public async Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto inventoryDto)
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

    public async Task<bool> IInventoryService.DeleteInventoryAsync(int inventoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<InventoryDto>> IInventoryService.GetInventoriesByWarehouseAsync(int inventoryId)
    {
        try
        {
            var inventories = await _context.Inventories.Where(w => w.WarehouseId.Equals(inventoryId)).ToListAsync();
            return inventories;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
