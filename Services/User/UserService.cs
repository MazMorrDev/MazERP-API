using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazErpBack.Dtos;
using MazErpBack.Dtos.Login;
using MazErpBack.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.User;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public UserService(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<TokenDto?> LoginUserAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Clients.FirstOrDefaultAsync(c => c.Email == loginDto.Email);
            if (user == null)
            {
                return null;
            }
            var hasher = new PasswordHasher<Client>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var token = _tokenService.CreateTokenAsync(user);
            return token;
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task<ClientDto> RegisterUserAsync(CreateUserDto userDto)
    {
        try
        {
            var client = new Client
            {
                Email = userDto.Email,
                Name = userDto.Name,
                ProfilePhotoUrl = userDto.ProfilePhotoUrl,
                CreatedAt = DateTimeOffset.UtcNow
            };
            var hasher = new PasswordHasher<Client>();
            client.PasswordHash = hasher.HashPassword(client, userDto.Password);
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            var response = new ClientDto(){
                Id = client.Id,
                Email = client.Email,
                Name = client.Name,
                ProfilePhotoUrl = client.ProfilePhotoUrl,
                CreatedAt = client.CreatedAt,
                UpdatedAt = client.UpdatedAt
            };
            return response;
        }
        catch (System.Exception)
        {
            // todo: implement logging
            throw;
        }
    }
}