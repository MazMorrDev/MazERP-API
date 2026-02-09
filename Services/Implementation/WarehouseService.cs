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
    public async Task<Warehouse> CreateWarehouse(CreateWarehouseDto warehouseDto)
    {
        try
        {
            var company = await _context.Companies.FindAsync(warehouseDto.CompanyId);
            ArgumentNullException.ThrowIfNull(company);

            var warehouse = new Warehouse()
            {
                CompanyId = warehouseDto.CompanyId,
                Name = warehouseDto.Name,
                Description = warehouseDto.Description,
                Company = company
            };

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto warehouseDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Warehouse> DeleteWarehouse(int id)
    {
        try
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            ArgumentNullException.ThrowIfNull(warehouse);

            warehouse.IsActive = false;
            await _context.SaveChangesAsync();

            return warehouse;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task DeleteWarehouseAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<WarehouseDto> GetWarehouseByIdAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public Task SoftDeleteWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public Task<WarehouseDto> UpdateWarehouseAsync(int WarehouseId, CreateWarehouseDto warehouseDto)
    {
        throw new NotImplementedException();
    }

    Task IWarehouseService.DeleteWarehouseAsync(int id)
    {
        return DeleteWarehouseAsync(id);
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
