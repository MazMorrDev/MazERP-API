using Microsoft.EntityFrameworkCore;

namespace MazErpBack;

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

    public async Task<Warehouse> DeleteWarehouse(DeleteWarehouseDto warehouseDto)
    {
        try
        {
            // Validación básica del DTO
            ArgumentNullException.ThrowIfNull(warehouseDto);

            var warehouse = await _context.Warehouses.FindAsync(warehouseDto.Id);
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

    public async Task<List<Warehouse>> GetWarehousesByWorkflowAsync(GetWarehousesByWfDto getWarehousesByWfDto)
    {
        try
        {
            // Validación básica del DTO
            ArgumentNullException.ThrowIfNull(getWarehousesByWfDto);

            var warehouses = await _context.Warehouses.Where(w => w.WorkflowId.Equals(getWarehousesByWfDto.WorkflowId)).ToListAsync();
            return warehouses;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
