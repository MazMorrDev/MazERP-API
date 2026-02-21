using MazErpBack.DTOs.Users;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IUserService
{
    public Task<UserDto> RegisterUserAsync(CreateUserDto userDto);

    public Task<TokenDto?> LoginUserAsync(LoginDto loginDto);

    public Task<User> GetUserByIdAsync(int id);
}