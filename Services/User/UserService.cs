using MazErpBack.Dtos;
using MazErpBack.Dtos.Login;
using MazErpBack.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.User;

public class UserService(AppDbContext context, TokenService tokenService) : IUserService
{
    private readonly AppDbContext _context = context;
    private readonly TokenService _tokenService = tokenService;

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
        catch (Exception)
        {
            // TODO
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
                ProfilePhotoUrl = userDto.ProfilePhotoUrl
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
        catch (Exception)
        {
            // TODO: implement logging
            throw;
        }
    }
}
