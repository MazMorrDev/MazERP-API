using MazErpBack.Dtos;
using MazErpBack.Dtos.Login;

namespace MazErpBack.Interfaces;

public interface IUserService
{
    public Task<ClientDto> RegisterUserAsync(CreateUserDto userDto);

    public Task<TokenDto?> LoginUserAsync(LoginDto loginDto);
}