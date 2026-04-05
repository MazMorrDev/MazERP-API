using MazErpAPI.Context;
using MazErpAPI.DTOs.Users;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MazErpAPI.Services.Implementation;

public class UserService(AppDbContext context, ITokenService tokenService, ILogger<UserService> logger, UserMapper mapper) : IUserService
{
    private readonly AppDbContext _context = context;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<UserService> _logger = logger;
    private readonly UserMapper _mapper = mapper;

    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null || !user.IsActive) throw new KeyNotFoundException($"User with id: {id} not found");
        return user;
    }

    public async Task<TokenDto?> LoginUserAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Users.Include(c => c.UserCompanies)
                .FirstOrDefaultAsync(c => c.Email == loginDto.Email && c.IsActive);

            if (user == null) return null;

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
        catch (Exception ex)
        {
            _logger.LogError($"It was an error: {ex.Message}");
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
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.MapToDto(user); ;
        }
        catch (Exception)
        {
            // TODO: implement logging
            throw;
        }
    }
}
