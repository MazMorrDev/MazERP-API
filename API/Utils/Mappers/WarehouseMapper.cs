using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class WarehouseMapper
{
    public WarehouseDto MapToDto(Warehouse warehouse)
    {
        return new WarehouseDto
        {
            WarehouseId = warehouse.Id,
            Name = warehouse.Name,
            Description = warehouse.Description,
            CompanyId = warehouse.CompanyId
        };
    }

    public Warehouse MapDtoToModel(Company company, CreateWarehouseDto dto)
    {
        return new Warehouse
        {
            CompanyId = dto.CompanyId,
            Name = dto.Name,
            Description = dto.Description,
            Company = company
        };
    }

    public List<WarehouseDto> MapListToDto(List<Warehouse> warehouses)
    {
        List<WarehouseDto> warehousesDto = [];
        foreach (var warehouse in warehouses)
        {
            warehousesDto.Add(MapToDto(warehouse));
        }
        return warehousesDto;
    }
}
