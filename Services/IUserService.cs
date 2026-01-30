using MazErpBack.Dtos.Users;

namespace MazErpBack.Services;

public interface IUserService
{
    public Task<UserDto> RegisterUserAsync(CreateUserDto userDto);

    public Task<TokenDto?> LoginUserAsync(LoginDto loginDto);
}