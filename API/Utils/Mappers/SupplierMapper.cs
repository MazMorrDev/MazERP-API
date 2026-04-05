using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;

namespace MazErpAPI.Utils.Mappers;

public class SupplierMapper
{
    public SupplierDto MapToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            SupplierId = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            PhoneNumber = supplier.Phone,
            Location = supplier.Location,
            Rating = supplier.Rating
        };
    }

    public Supplier MapDtoToModel(CreateSupplierDto supplierDto)
    {
        return new Supplier
        {
            Name = supplierDto.Name,
            ContactPerson = supplierDto.ContactPerson,
            Email = supplierDto.Email,
            Phone = supplierDto.PhoneNumber,
            Location = supplierDto.Location,
            Rating = supplierDto.Rating
        };
    }

    public List<SupplierDto> MapListToDto(List<Supplier> suppliers)
    {
        List<SupplierDto> companiesDto = [];
        foreach (var supplier in suppliers)
        {
            companiesDto.Add(MapToDto(supplier));
        }
        return companiesDto;
    }
}
