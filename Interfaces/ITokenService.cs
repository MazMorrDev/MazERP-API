using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazErpBack.Dtos.Login;

namespace MazErpBack.Interfaces;

public interface ITokenService
{
    public TokenDto CreateTokenAsync(Client client);
}