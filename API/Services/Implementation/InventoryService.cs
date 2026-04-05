using MazErpAPI.Context;
using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpAPI.Services.Implementation;

public class InventoryService(
    AppDbContext context, InventoryMapper mapper, IProductService productService,
    IWarehouseService warehouseService, ILogger<InventoryService> logger) : IInventoryService
{
    private readonly AppDbContext _context = context;
    private readonly InventoryMapper _mapper = mapper;
    private readonly IProductService _productService = productService;
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly ILogger<InventoryService> _logger = logger;

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

    public async Task<PaginatedResult<InventoryDto>> GetInventoriesByWarehouseAsync(int warehouseId, int pageNumber, int pageSize)
    {
        try
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(warehouseId);
            var inventories = warehouse.Inventories.ToList();
            var products = inventories.Select(i => i.Product).ToList();
            var inventoriesDto = _mapper.MapListToDto(warehouse.Inventories.ToList(), products);
            var totalCount = inventories.Count;

            return new PaginatedResult<InventoryDto>(inventoriesDto, totalCount, pageNumber, pageSize);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PaginatedResult<InventoryDto>> GetInventoriesByCompanyAsync(int companyId, int pageNumber, int pageSize)
    {
        var query = _context.Warehouses.Where(c => c.CompanyId == companyId && c.IsActive).SelectMany(c => c.Inventories);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        List<InventoryDto> inventoriesDto = [];
        foreach (var item in items)
        {
            inventoriesDto.Add(_mapper.MapToDto(item, item.Product));
        }

        return new PaginatedResult<InventoryDto>(inventoriesDto, totalCount, pageNumber, pageSize);
    }
}
