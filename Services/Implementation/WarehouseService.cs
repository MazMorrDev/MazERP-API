using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class WarehouseService(AppDbContext context, WarehouseMapper mapper) : IWarehouseService
{
    private readonly AppDbContext _context = context;
    private readonly WarehouseMapper _mapper = mapper;
    public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto warehouseDto)
    {
        try
        {
            var company = await _context.Companies.FindAsync(warehouseDto.CompanyId);
            ArgumentNullException.ThrowIfNull(company);

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
        ArgumentNullException.ThrowIfNull(warehouse);
        return warehouse;
    }

    public async Task SoftDeleteWarehouseAsync(int warehouseId)
    {
        var warehouse = await GetWarehouseByIdAsync(warehouseId);
        warehouse.IsActive = false;
        await _context.SaveChangesAsync();
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
            var warehouses = await _context.Warehouses.Where(w => w.CompanyId.Equals(companyId)).ToListAsync();
            return _mapper.MapListToDto(warehouses);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
