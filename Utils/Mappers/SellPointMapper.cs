using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class SellPointMapper
{
    public SellPointDto MaptoDto(SellPoint sellPoint)
    {
        return new SellPointDto
        {
            Id = sellPoint.Id,
            Name = sellPoint.Name,
            Description = sellPoint.Description,
            Location = sellPoint.Location
        };
    }
}
