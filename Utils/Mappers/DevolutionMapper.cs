using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class DevolutionMapper
{
    public DevolutionDto MapToDto(Devolution devolution)
    {
        var devolutionDto = new DevolutionDto
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
        return devolutionDto;
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
}
