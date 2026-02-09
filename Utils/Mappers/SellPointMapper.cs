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

    public SellPoint MapDtoToModel(CreateSellPointDto sellPointDto)
    {
        return new SellPoint
        {
            Name = sellPointDto.Name,
            Description = sellPointDto.Description,
            Location = sellPointDto.Location,
        };
    }

    public List<SellPointDto> MapListToDto(List<SellPoint> sellPoints)
    {
        List<SellPointDto> sellPointsDto = [];
        foreach (var sellPoint in sellPoints)
        {
            sellPointsDto.Add(MaptoDto(sellPoint));
        }
        return sellPointsDto;
    }
}
