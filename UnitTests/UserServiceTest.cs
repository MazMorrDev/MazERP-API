using MazErpAPI.Context;
using MazErpAPI.DTOs.Users;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class UserServiceTest
{
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly UserMapper _mapper;
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _mockLogger = new Mock<ILogger<UserService>>();
        _mockTokenService = new Mock<ITokenService>();
        _mapper = new UserMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _userService = new UserService(_context, _mockTokenService.Object, _mockLogger.Object, _mapper);
    }

    #region RegisterUserAsync Tests

    [Fact]
    public async Task RegisterUser_WithValidData_ReturnsUserDto()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "newuser@example.com",
            Name = "New User",
            Password = "SecurePass123!",
            ProfilePhotoUrl = "https://example.com/photo.jpg"
        };

        // Act
        var result = await _userService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createUserDto.Email, result.Email);
        Assert.True(result.Id > 0);

        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == createUserDto.Email);
        Assert.NotNull(savedUser);
        Assert.NotNull(savedUser.PasswordHash);
        Assert.NotEqual(createUserDto.Password, savedUser.PasswordHash);
        Assert.True(savedUser.IsActive);
    }

    [Fact]
    public async Task RegisterUser_WithDuplicateEmail_ThrowsException()
    {
        // Arrange
        var existingUser = new User
        {
            Email = "duplicate@example.com",
            Name = "Existing User",
            PasswordHash = "somehash"
        };
        await _context.Users.AddAsync(existingUser);
        await _context.SaveChangesAsync();

        var createUserDto = new CreateUserDto
        {
            Email = "duplicate@example.com",
            Name = "New User",
            Password = "SecurePass123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() =>
            _userService.RegisterUserAsync(createUserDto));
    }

    [Fact]
    public async Task RegisterUser_WithNullEmail_ThrowsException()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = null!,
            Name = "New User",
            Password = "SecurePass123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() =>
            _userService.RegisterUserAsync(createUserDto));
    }

    #endregion

    #region GetUserByIdAsync Tests

    [Fact]
    public async Task GetUserById_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            Name = "Test User",
            IsActive = true,
            PasswordHash = "hash"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task GetUserById_WithInactiveOrNonExistentUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var inactiveUser = new User
        {
            Email = "inactive@example.com",
            Name = "Inactive User",
            IsActive = false,
            PasswordHash = "hash"
        };
        await _context.Users.AddAsync(inactiveUser);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserByIdAsync(inactiveUser.Id));
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserByIdAsync(99999));
    }

    #endregion

    #region LoginUserAsync Tests

    [Fact]
    public async Task LoginUser_WithValidCredentials_ReturnsTokenDto()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var userDto = _mapper.MapToDto(user);
        var expectedToken = new TokenDto { Token = "test-token-123", Expiration = DateTime.Now.AddHours(1), UserDto = userDto };
        _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>())).Returns(expectedToken);

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TokenDto>(result);
    }

    [Fact]
    public async Task LoginUser_WithInvalidCredentials_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true
        };
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "wrong@example.com",
            Password = "wrongpassword"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginUser_WithInactiveUser_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "inactive@example.com",
            Name = "inactiveuser",
            IsActive = false
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "inactive@example.com",
            Password = "password123"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
        _mockTokenService.Verify(x => x.CreateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginUser_WithNullEmailOrPassword_ThrowsException()
    {
        // Arrange
        var loginDto1 = new LoginDto { Email = null!, Password = "password123" };
        var loginDto2 = new LoginDto { Email = "test@example.com", Password = null! };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _userService.LoginUserAsync(loginDto1));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _userService.LoginUserAsync(loginDto2));
    }

    [Fact]
    public async Task LoginUser_WithUserHavingMultipleCompanies_LoadsCompaniesCorrectly()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            UserCompanies =
            [
                new UserCompany { CompanyId = 1, UserId = 1 },
                new UserCompany { CompanyId = 2, UserId = 1 }
            ]
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        User capturedUser = null!;
        _mockTokenService
            .Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns(new TokenDto { Token = "test-token" })
            .Callback<User>(u => capturedUser = u);

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(capturedUser);
        Assert.Equal(2, capturedUser.UserCompanies?.Count);
    }

    [Fact]
    public async Task LoginUser_WhenPasswordVerificationFails_LogsWarning()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "correctpassword");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Failed login attempt")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion
}