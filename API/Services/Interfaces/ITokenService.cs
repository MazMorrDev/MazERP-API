using MazErpAPI.DTOs.Users;
using MazErpAPI.Models;

namespace MazErpAPI.Services.Interfaces;

public interface ITokenService
{
    public TokenDto CreateToken(User user);
}