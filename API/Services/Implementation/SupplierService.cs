using MazErpAPI.Context;
using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpBack.Utils;
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

    public async Task<PaginatedResult<SupplierDto>> GetSuppliersByWarehouseAsync(int warehouseId, int pageNumber, int pageSize)
    {
        var query = _context.Inventories
            .Where(i => i.WarehouseId == warehouseId && i.IsActive)
            .SelectMany(i => i.InventorySuppliers)
            .Select(si => si.Supplier).Distinct();
        var totalCount = await query.CountAsync();
        var suppliers = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();
        var suppliersDto = suppliers.Select(_mapper.MapToDto).ToList();

        return new PaginatedResult<SupplierDto>(suppliersDto, totalCount, pageNumber, pageSize);
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
