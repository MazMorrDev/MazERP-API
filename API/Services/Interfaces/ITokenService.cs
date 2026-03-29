using MazErpBack.DTOs.Users;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface ITokenService
{
    public TokenDto CreateToken(User user);
}