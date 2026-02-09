using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class WarehouseMapper
{
    public WarehouseDto MapToDto(Warehouse warehouse)
    {
        return new WarehouseDto
        {
            Name = warehouse.Name,
            WarehouseId = warehouse.Id,
            Description = warehouse.Description,
            CompanyId = warehouse.CompanyId
        };
    }
}
