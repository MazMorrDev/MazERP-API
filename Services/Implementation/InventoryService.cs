using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class InventoryService(AppDbContext context, InventoryMapper mapper, IProductService productService) : IInventoryService
{
    private readonly AppDbContext _context = context;
    private readonly InventoryMapper _mapper = mapper;
    private readonly IProductService _productService = productService;

    public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
    {
        return await _context.Inventories.FindAsync(inventoryId) ?? throw new KeyNotFoundException($"Inventory with id: {inventoryId} not found");
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

    public async Task<InventoryDto> UpdateInventoryAsync(int inventoryId, CreateInventoryExistentProductDto inventoryDto)
    {
        var inventory = await GetInventoryByIdAsync(inventoryId);

        inventory.AlertStock = inventoryDto.AlertStock;
        inventory.WarehouseId = inventoryDto.WarehouseId;
        inventory.ProductId = inventoryDto.ProductId;
        inventory.Stock = inventoryDto.Stock;
        inventory.BasePrice = inventoryDto.BasePrice;
        inventory.BaseDiscount = inventoryDto.BaseDiscount;
        inventory.AverageCost = inventoryDto.AverageCost;
        inventory.AlertStock = inventoryDto.AlertStock;
        inventory.AlertStock = inventoryDto.AlertStock;

        await _context.SaveChangesAsync();
        return _mapper.MapToDto(inventory);
    }

    public async Task<InventoryDto> CreateInventoryForExistentProductAsync(CreateInventoryExistentProductDto inventoryDto)
    {
        try
        {
            var warehouse = await _context.Warehouses.FindAsync(inventoryDto.WarehouseId) ?? throw new KeyNotFoundException($"Warehouse with id: {inventoryDto.WarehouseId} not found");
            var product = await _productService.GetProductByIdAsync(inventoryDto.ProductId);

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

    public async Task<InventoryDto> CreateInventoryForNewProductAsync(CreateInventoryNewProductDto inventoryDto, CreateProductDto productDto)
    {
        try
        {
            var product = await _productService.CreateProductAsync(productDto);
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
            inventory.IsActive = false;
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
