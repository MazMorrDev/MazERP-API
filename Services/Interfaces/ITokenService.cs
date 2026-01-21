using MazErpBack.Dtos.Users;

namespace MazErpBack.Services.Interfaces;

public interface ITokenService
{
    public TokenDto CreateTokenAsync(User user);
}