namespace MazErpBack.Utils.Mappers;

public interface IMapper<Dto, CreateDto, Model>
{
    public Dto MapToDto(Model model);
    public Model MapDtoToModel(CreateDto createDto);
    public List<Dto> MapListToDto(List<Model> model);
}
