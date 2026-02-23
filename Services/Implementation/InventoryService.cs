using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class InventoryService(AppDbContext context, InventoryMapper mapper, IProductService productService, IWarehouseService warehouseService) : IInventoryService
{
    private readonly AppDbContext _context = context;
    private readonly InventoryMapper _mapper = mapper;
    private readonly IProductService _productService = productService;
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
    {
        var inventory = await _context.Inventories.FindAsync(inventoryId);
        if (inventory == null || !inventory.IsActive) throw new KeyNotFoundException($"Inventory with id: {inventoryId} not found");
        return inventory;
    }

    public async Task<bool> SoftDeleteInventoryAsync(int inventoryId)
    {
        try
        {
            var inventory = await GetInventoryByIdAsync(inventoryId);
            inventory.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }

    }

    public async Task<InventoryDto> UpdateInventoryAndProductAsync(int inventoryId, UpdateInventoryProductDto inventoryDto)
    {
        var inventory = await GetInventoryByIdAsync(inventoryId);
        var product = await _productService.GetProductByIdAsync(inventoryDto.ProductId);

        inventory.AlertStock = inventoryDto.AlertStock;
        inventory.WarehouseId = inventoryDto.WarehouseId;
        inventory.ProductId = inventoryDto.ProductId;
        inventory.Stock = inventoryDto.Stock;
        inventory.BasePrice = inventoryDto.BasePrice;
        inventory.BaseDiscount = inventoryDto.BaseDiscount;
        inventory.AverageCost = inventoryDto.AverageCost;
        inventory.AlertStock = inventoryDto.AlertStock;
        inventory.AlertStock = inventoryDto.AlertStock;

        await _productService.UpdateProductAsync(product.Id, inventoryDto);
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(inventory, product);
    }

    public async Task<InventoryDto> CreateInventoryByExistentProductAsync(CreateInventoryByExistentProductDto inventoryDto)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(inventoryDto.ProductId);
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(inventoryDto.WarehouseId);
            var inventory = _mapper.MapDtoByProductToModel(warehouse, product, inventoryDto);

            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
            return _mapper.MapToDto(inventory, product);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<InventoryDto> CreateInventoryAndProductAsync(CreateInventoryAndProductDto inventoryDto)
    {
        try
        {
            var product = await _productService.CreateProductAsync(inventoryDto);
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(inventoryDto.WarehouseId);
            var inventory = _mapper.MapDtoToModel(warehouse, product, inventoryDto);

            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
            return _mapper.MapToDto(inventory, product);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteInventoryAsync(int inventoryId)
    {
        try
        {
            var inventory = await GetInventoryByIdAsync(inventoryId);
            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
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
            var inventories = await _context.Inventories.Where(i => i.WarehouseId.Equals(warehouseId) && i.IsActive).ToListAsync();
            List<Product> products = [];
            foreach (var item in inventories)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                products.Add(product);
            }

            return _mapper.MapListToDto(inventories, products);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<InventoryDto>> GetInventoriesByCompanyAsync(int companyId)
    {
        var warehouses = await _context.Warehouses.Where(c => c.CompanyId.Equals(companyId) && c.IsActive).ToListAsync();
        List<InventoryDto> inventoriesDto = [];
        foreach (var warehouse in warehouses)
        {
            inventoriesDto.AddRange(await GetInventoriesByWarehouseAsync(warehouse.Id));
        }
        return inventoriesDto;
    }
}
