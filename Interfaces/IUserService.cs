using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazErpBack.Dtos;
using MazErpBack.Dtos.Login;

namespace MazErpBack.Interfaces;

public interface IUserService
{
    public Task<ClientDto> RegisterUserAsync(CreateUserDto userDto);

    public Task<TokenDto?> LoginUserAsync(LoginDto loginDto);
}