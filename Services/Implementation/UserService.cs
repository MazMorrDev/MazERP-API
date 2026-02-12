using MazErpBack.Context;
using MazErpBack.DTOs.Users;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class UserService(AppDbContext context, ITokenService tokenService, ILogger<UserService> logger, UserMapper mapper) : IUserService
{
    private readonly AppDbContext _context = context;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<UserService> _logger = logger;
    private readonly UserMapper _mapper = mapper;

    public async Task<TokenDto?> LoginUserAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Users
                .Include(c => c.UserCompanies)
                .FirstOrDefaultAsync(c => c.Email == loginDto.Email);
            if (user == null)
            {
                return null;
            }
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);
                return null;
            }
            var token = _tokenService.CreateToken(user);
            _logger.LogInformation("User logged in: {Email}", loginDto.Email);
            return token;
        }
        catch (Exception)
        {
            // TODO:
            throw;
        }
    }

    public async Task<UserDto> RegisterUserAsync(CreateUserDto userDto)
    {
        try
        {
            var user = new User
            {
                Email = userDto.Email,
                Name = userDto.Name,
                ProfilePhotoUrl = userDto.ProfilePhotoUrl
            };
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, userDto.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // TODO: register wf a user - modificar service para hacerlo en un solo paso

            return _mapper.MapToDto(user);;
        }
        catch (Exception)
        {
            // TODO: implement logging
            throw;
        }
    }
}
