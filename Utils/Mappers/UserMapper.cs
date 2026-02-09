using MazErpBack.DTOs.Users;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class UserMapper
{
    public UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ProfilePhotoUrl = user.ProfilePhotoUrl
        };
    }

    public User MapDtoToModel(CreateUserDto userDto)
    {
        return new User
        {
            Name = userDto.Name,
            Description = userDto.Description,
            UserPhotoUrl = userDto.UserPhotoUrl,
            Currency = userDto.Currency,
            CreatedAt = userDto.CreatedAt
        };
    }

    public List<UserDto> MapListToDto(List<User> users)
    {
        List<UserDto> companiesDto = [];
        foreach (var user in users)
        {
            companiesDto.Add(MapToDto(user));
        }
        return companiesDto;
    }
}
