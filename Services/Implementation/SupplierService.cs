using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class SupplierService(AppDbContext context, SupplierMapper mapper, ILogger<SupplierService> logger) : ISupplierService
{
    private readonly AppDbContext _context = context;
    private readonly SupplierMapper _mapper = mapper;
    private readonly ILogger<SupplierService> _logger = logger;

    public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto supplierDto)
    {
        var supplier = _mapper.MapDtoToModel(supplierDto);
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        return _mapper.MapToDto(supplier);
    }

    public async Task DeleteSupplierAsync(int supplierId)
    {
        try
        {
            var supplier = await GetSupplierByIdAsync(supplierId);
            _context.Suppliers.Remove(supplier);
            var result = await _context.SaveChangesAsync() > 0;

            if (result)
                _logger.LogInformation("Devolución {Id} eliminada", supplierId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar devolución {Id}", supplierId);
            throw;
        }
    }

    public async Task<Supplier> GetSupplierByIdAsync(int supplierId)
    {
        try
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null || !supplier.IsActive) throw new KeyNotFoundException($"Supplier with id: {supplierId} not found");
            return supplier;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            throw;
        }

    }

    public async Task<List<SupplierDto>> GetSuppliersByWarehouseAsync(int warehouseId)
    {
        var inventories = await _context.Inventories.Where(i => i.WarehouseId == warehouseId).ToListAsync();
        List<SupplierDto> suppliersDto = [];
        foreach (var i in inventories)
        {
            var inventorySuppliers = await _context.InventorySuppliers.Where(invSup => invSup.InventoryId == i.Id).ToListAsync();
            foreach (var invSup in inventorySuppliers)
            {
                var suppliers = await _context.Suppliers.Where(s => s.Id == invSup.SupplierId).ToListAsync();
                suppliersDto.AddRange(_mapper.MapListToDto(suppliers));
            }
        }
        return suppliersDto;
    }

    public async Task<bool> SoftDeleteSupplierAsync(int supplierId)
    {
        try
        {
            var supplier = await GetSupplierByIdAsync(supplierId);
            supplier.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }

    }

    public async Task<SupplierDto> UpdateSupplierAsync(int supplierId, CreateSupplierDto supplierDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var supplier = await GetSupplierByIdAsync(supplierId);
            ArgumentNullException.ThrowIfNull(supplier);

            supplier.Name = supplierDto.Name;
            supplier.ContactPerson = supplierDto.ContactPerson;
            supplier.Email = supplierDto.Email;
            supplier.Phone = supplierDto.PhoneNumber;
            supplier.Location = supplierDto.Location;
            supplier.Rating = supplierDto.Rating;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.MapToDto(supplier);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}
