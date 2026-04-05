using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;

namespace MazErpAPI.Utils.Mappers;

public class DevolutionMapper
{
    public DevolutionDto MapToDto(Devolution devolution)
    {
        return new DevolutionDto
        {
            DevolutionId = devolution.Id,
            SellId = devolution.SellId,
            Reason = devolution.Reason,
            RefundAmount = devolution.RefundAmount,
            Notes = devolution.Notes,
            DevolutionStatus = devolution.DevolutionStatus,
            DevolutionActionTake = devolution.DevolutionActionTake,
            Date = devolution.DevolutionDate
        };
    }

    public List<DevolutionDto> MapListToDto(List<Devolution> devolutions)
    {
        List<DevolutionDto> devolutionsDto = [];
        foreach (var devolution in devolutions)
        {
            devolutionsDto.Add(MapToDto(devolution));
        }
        return devolutionsDto;
    }

    public Devolution MapDtoToModel(Sell sell, CreateDevolutionDto createDto)
    {
        return new Devolution
        {
            SellId = createDto.SellId,
            Reason = createDto.Reason,
            RefundAmount = createDto.RefundAmount,
            Notes = createDto.Notes,
            DevolutionStatus = createDto.DevolutionStatus,
            DevolutionActionTake = createDto.DevolutionActionTake,
            DevolutionDate = createDto.Date,
            Sell = sell,
        };
    }
}
