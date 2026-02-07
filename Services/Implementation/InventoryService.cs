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
            var warehouse = await _context.Warehouses.FindAsync(inventoryDto.WarehouseId);
            var product = await _context.Products.FindAsync(inventoryDto.ProductId);


            if (warehouse == null || product == null)
            {
                // logging
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

    public async Task<List<InventoryDto>> GetInventoriesByWarehouseAsync(int inventoryId)
    {
        try
        {
            var inventories = await _context.Inventories.Where(w => w.WarehouseId.Equals(inventoryId)).ToListAsync();
            return _mapper.MapListToDto(inventories);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
