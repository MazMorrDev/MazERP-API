using MazErpBack.Dtos.Login;

namespace MazErpBack.Interfaces;

public interface ITokenService
{
    public TokenDto CreateTokenAsync(Client client);
}