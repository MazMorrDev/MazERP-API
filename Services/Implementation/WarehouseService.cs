using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class WarehouseService(AppDbContext context, WarehouseMapper mapper, ICompanyService companyService) : IWarehouseService
{
    private readonly AppDbContext _context = context;
    private readonly WarehouseMapper _mapper = mapper;
    private readonly ICompanyService _companyService = companyService;
    public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto warehouseDto)
    {
        try
        {
            var company = await _companyService.GetCompanyByIdAsync(warehouseDto.CompanyId);

            var warehouse = _mapper.MapDtoToModel(company, warehouseDto);

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return _mapper.MapToDto(warehouse);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteWarehouseAsync(int id)
    {
        try
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            ArgumentNullException.ThrowIfNull(warehouse);

            warehouse.IsActive = false;
            await _context.SaveChangesAsync();

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Warehouse> GetWarehouseByIdAsync(int warehouseId)
    {
        var warehouse = await _context.Warehouses.FindAsync(warehouseId);
        if (warehouse == null || !warehouse.IsActive) throw new KeyNotFoundException($"Warehouse with id: {warehouseId} not found");
        return warehouse;
    }

    public async Task<bool> SoftDeleteWarehouseAsync(int warehouseId)
    {
        try
        {
            var warehouse = await GetWarehouseByIdAsync(warehouseId);
            warehouse.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
            throw;
        }

    }

    public async Task<WarehouseDto> UpdateWarehouseAsync(int warehouseId, CreateWarehouseDto warehouseDto)
    {
        var warehouse = await GetWarehouseByIdAsync(warehouseId);
        warehouse.CompanyId = warehouseDto.CompanyId;
        warehouse.Name = warehouseDto.Name;
        warehouse.Description = warehouseDto.Description;
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(warehouse);
    }

    public async Task<List<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId)
    {
        try
        {
            var warehouses = await _context.Warehouses.Where(w => w.CompanyId == companyId && w.IsActive).ToListAsync();
            return _mapper.MapListToDto(warehouses);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
