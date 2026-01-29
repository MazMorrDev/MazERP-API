using MazErpBack.Context;
using MazErpBack.Dtos.Warehouse;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services;

public class WarehouseService(AppDbContext context) : IWarehouseService
{
    private readonly AppDbContext _context = context;
    public async Task<Warehouse> CreateWarehouse(CreateWarehouseDto warehouseDto)
    {
        try
        {
            // Validation
            ArgumentNullException.ThrowIfNull(warehouseDto);

            var warehouse = new Warehouse()
            {
                WorkflowId = warehouseDto.WorkflowId,
                Name = warehouseDto.Name,
                Description = warehouseDto.Description
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

    public async Task<List<Warehouse>> GetWarehousesByWorkflowAsync(int workflowId)
    {
        try
        {
            var warehouses = await _context.Warehouses.Where(w => w.WorkflowId.Equals(workflowId)).ToListAsync();
            return warehouses;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
