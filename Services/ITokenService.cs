using MazErpBack.DTOs.Users;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ITokenService
{
    public TokenDto CreateTokenAsync(User user);
}