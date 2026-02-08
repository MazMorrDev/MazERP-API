using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class DevolutionMapper: IMapper<DevolutionDto, CreateDevolutionDto, Devolution>
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

    public Devolution MapDtoToModel(CreateDevolutionDto createDto)
    {
        throw new NotImplementedException();
    }
}
