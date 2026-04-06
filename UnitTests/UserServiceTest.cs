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

        // Configurar base de datos en memoria
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _userService = new UserService(_context, _mockTokenService.Object, _mockLogger.Object, _mapper);
    }

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
        Assert.Equal(createUserDto.Name, result.Name);
        Assert.Equal(createUserDto.ProfilePhotoUrl, result.ProfilePhotoUrl);
        Assert.True(result.Id > 0);

        // Verificar que el usuario se guardó en la BD
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == createUserDto.Email);
        Assert.NotNull(savedUser);
        Assert.NotNull(savedUser.PasswordHash);
        Assert.NotEqual(createUserDto.Password, savedUser.PasswordHash); // La contraseña debe estar hasheada
        Assert.True(savedUser.IsActive); // Por defecto debería estar activo
    }

    [Fact]
    public async Task RegisterUser_WithDuplicateEmail_ThrowsException()
    {
        // Arrange - Crear usuario existente
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

    [Fact]
    public async Task RegisterUser_WithEmptyName_UsesEmptyString()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "noname@example.com",
            Name = "",
            Password = "SecurePass123!"
        };

        // Act
        var result = await _userService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("", result.Name);
    }

    [Fact]
    public async Task RegisterUser_WithNullProfilePhotoUrl_SavesNull()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "nophoto@example.com",
            Name = "No Photo User",
            Password = "SecurePass123!",
            ProfilePhotoUrl = null
        };

        // Act
        var result = await _userService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ProfilePhotoUrl);
    }

    [Fact]
    public async Task RegisterUser_WithVeryLongName_TruncatesOrSaves()
    {
        // Arrange
        var longName = new string('a', 500);
        var createUserDto = new CreateUserDto
        {
            Email = "longname@example.com",
            Name = longName,
            Password = "SecurePass123!"
        };

        // Act
        var result = await _userService.RegisterUserAsync(createUserDto);

        // Assert - Depende de la configuración de tu BD
        Assert.NotNull(result);
        Assert.Equal(longName, result.Name);
    }

    [Fact]
    public async Task RegisterUser_WithWeakPassword_StillSaves()
    {
        // Arrange - Tu método no valida fortaleza de contraseña
        var createUserDto = new CreateUserDto
        {
            Email = "weakpass@example.com",
            Name = "Weak Password User",
            Password = "123"
        };

        // Act
        var result = await _userService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.NotNull(result);
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "weakpass@example.com");
        Assert.NotNull(savedUser);
    }

    [Fact]
    public async Task RegisterUser_WithSpecialCharactersInEmail_SavesCorrectly()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "test+special@example.com",
            Name = "Special Email User",
            Password = "SecurePass123!"
        };

        // Act
        var result = await _userService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test+special@example.com", result.Email);
    }

    [Fact]
    public async Task RegisterUser_WhenDatabaseThrowsException_PropagatesException()
    {
        // Arrange
        await _context.DisposeAsync();
        var createUserDto = new CreateUserDto
        {
            Email = "test@example.com",
            Name = "Test User",
            Password = "SecurePass123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(() =>
            _userService.RegisterUserAsync(createUserDto));
    }

    [Fact]
    public async Task RegisterUser_DoesNotSetLicenseDates()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "license@example.com",
            Name = "License Test User",
            Password = "SecurePass123!"
        };

        // Act
        var result = await _userService.RegisterUserAsync(createUserDto);

        // Assert
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "license@example.com");
        Assert.NotNull(savedUser);
        Assert.Equal(default, savedUser.LicenseStartDate);
        Assert.Equal(default, savedUser.LicenseEndDate);
    }

    [Fact]
    public async Task RegisterUser_GeneratesUniqueIdForEachUser()
    {
        // Arrange
        var user1Dto = new CreateUserDto
        {
            Email = "user1@example.com",
            Name = "User One",
            Password = "Pass123!"
        };
        var user2Dto = new CreateUserDto
        {
            Email = "user2@example.com",
            Name = "User Two",
            Password = "Pass123!"
        };

        // Act
        var result1 = await _userService.RegisterUserAsync(user1Dto);
        var result2 = await _userService.RegisterUserAsync(user2Dto);

        // Assert
        Assert.NotEqual(result1.Id, result2.Id);
    }

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
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserById_WithInactiveUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var user = new User
        {
            Email = "inactive@example.com",
            Name = "Inactive User",
            IsActive = false,
            PasswordHash = "hash"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserByIdAsync(user.Id));

        Assert.Contains($"User with id: {user.Id} not found", exception.Message);
    }

    [Fact]
    public async Task GetUserById_WithNonExistentId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserByIdAsync(99999));

        Assert.Contains("User with id: 99999 not found", exception.Message);
    }

    [Fact]
    public async Task GetUserById_WithZeroId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserByIdAsync(0));
    }

    [Fact]
    public async Task GetUserById_WithNegativeId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserByIdAsync(-1));
    }

    [Fact]
    public async Task GetUserById_WhenUserExistsButIsDeleted_ThrowsKeyNotFoundException()
    {
        // Arrange - Simulando soft delete
        var user = new User
        {
            Email = "deleted@example.com",
            Name = "Deleted User",
            IsActive = false, // Usuario desactivado (soft delete)
            PasswordHash = "hash"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserByIdAsync(user.Id));
    }

    [Fact]
    public async Task LoginUser_WithValidCredentials_IncludesUserCompaniesInToken()
    {
        // Arrange
        var company1 = new Company { Id = 1, Name = "Company 1" };
        var company2 = new Company { Id = 2, Name = "Company 2" };

        var user = new User
        {
            Email = "multicompany@example.com",
            Name = "Multi Company User",
            IsActive = true,
            PasswordHash = new PasswordHasher<User>().HashPassword(null!, "password123"),
            UserCompanies = new List<UserCompany>
        {
            new UserCompany { CompanyId = 1, UserId = 1 },
            new UserCompany { CompanyId = 2, UserId = 1 }
        }
        };

        await _context.Companies.AddRangeAsync(company1, company2);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        User capturedUser = null!;
        _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns(new TokenDto { Token = "test-token" })
            .Callback<User>(u => capturedUser = u);

        var loginDto = new LoginDto
        {
            Email = "multicompany@example.com",
            Password = "password123"
        };

        // Act
        await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.NotNull(capturedUser.UserCompanies);
        Assert.Equal(2, capturedUser.UserCompanies.Count);
    }

    [Fact]
    public async Task LoginUser_WithValidCredentials_DoesNotLoadUserCompanies_WhenNotNeeded()
    {
        // Arrange
        var user = new User
        {
            Email = "nocompanies@example.com",
            Name = "No Companies User",
            IsActive = true,
            PasswordHash = new PasswordHasher<User>().HashPassword(null!, "password123")
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        User capturedUser = null!;
        _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns(new TokenDto { Token = "test-token" })
            .Callback<User>(u => capturedUser = u);

        var loginDto = new LoginDto
        {
            Email = "nocompanies@example.com",
            Password = "password123"
        };

        // Act
        await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.Null(capturedUser.UserCompanies); // O empty, según implementación
    }

    [Fact]
    public async Task LoginUser_WithCorrectCredentials_ReturnsTokenDto()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
        };

        // IMPORTANTE: Usar el mismo PasswordHasher que usa el servicio
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Verificar que el usuario se guardó correctamente
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(savedUser); // Debug: verificar que existe
        Assert.True(savedUser.IsActive); // Debug: verificar que está activo
        Assert.NotNull(savedUser.PasswordHash); // Debug: verificar que tiene hash

        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var userDto = _mapper.MapToDto(user);
        // Setup mock token service para que retorne algo
        var expectedToken = new TokenDto { Token = "test-token-123", Expiration = DateTime.Now.AddHours(1), UserDto = userDto };
        _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>())).Returns(expectedToken);

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.NotNull(result); // Cambiar a Assert.NotNull primero
        Assert.IsType<TokenDto>(result);
    }


    [Fact]
    public async Task LoginUser_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
        };
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "pepito@gmail.com",
            Password = "password123"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginUser_WithUnexistentEmail_ReturnsNull()
    {
        var loginDto = new LoginDto
        {
            Email = "pepito@gmail.com",
            Password = "password123"
        };

        // Execution
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginUser_WithInactiveUser_ReturnsNull()
    {
        // Arrange - Usuario INACTIVO
        var user = new User
        {
            Id = 1,
            Email = "inactive@example.com",
            Name = "inactiveuser",
            IsActive = false, // ← CLAVE: Usuario inactivo
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
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

        // Verificar que NO se llamó a TokenService
        _mockTokenService.Verify(x => x.CreateToken(It.IsAny<User>()), Times.Never);

        // Verificar el log de advertencia
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Failed login attempt")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never); // No debería haber log de warning porque el usuario ni siquiera se encontró
    }

    [Fact]
    public async Task LoginUser_WithExpiredLicense_StillWorks_IfUserIsActive()
    {
        // Arrange - Licencia EXPIRADA pero usuario activo
        var user = new User
        {
            Id = 1,
            Email = "expired@example.com",
            Name = "expireduser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow.AddMonths(-2),
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(-1) // Expirada hace 1 mes
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "expired@example.com",
            Password = "password123"
        };

        var userDto = _mapper.MapToDto(user);
        var expectedToken = new TokenDto { Token = "test-token", Expiration = DateTime.Now.AddHours(1), UserDto = userDto };
        _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>())).Returns(expectedToken);

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert - NOTA: Tu método NO valida fechas de licencia, así que debería funcionar
        Assert.NotNull(result);
        Assert.IsType<TokenDto>(result);
    }

    [Fact]
    public async Task LoginUser_WithNullEmail_ThrowsException()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = null!, // Email nulo
            Password = "password123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _userService.LoginUserAsync(loginDto));
    }

    [Fact]
    public async Task LoginUser_WithEmptyEmail_ThrowsException()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "", // Email vacío
            Password = "password123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _userService.LoginUserAsync(loginDto));
    }

    [Fact]
    public async Task LoginUser_WithNullPassword_ThrowsException()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = null! // Password nulo
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _userService.LoginUserAsync(loginDto));
    }

    [Fact]
    public async Task LoginUser_WhenTokenServiceThrowsException_PropagatesException()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
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

        // Configurar TokenService para que lance excepción
        _mockTokenService
            .Setup(x => x.CreateToken(It.IsAny<User>()))
            .ThrowsAsync(new InvalidOperationException("Token generation failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.LoginUserAsync(loginDto));

        Assert.Equal("Token generation failed", exception.Message);

        // Verificar que se logueó el error
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("It was an error")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task LoginUser_WhenDatabaseThrowsException_PropagatesException()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        // Forzar un error en la BD (ejemplo: disposición del contexto)
        await _context.DisposeAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(() =>
            _userService.LoginUserAsync(loginDto));
    }

    [Fact]
    public async Task LoginUser_WithCaseSensitiveEmail_ShouldMatchExactly()
    {
        // Arrange - Usuario con email en minúsculas
        var user = new User
        {
            Id = 1,
            Email = "test@example.com", // Minúsculas
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "TEST@EXAMPLE.COM", // Mayúsculas
            Password = "password123"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert - Depende de si tu BD es case-sensitive o no
        // SQL Server por defecto NO es case-sensitive, PostgreSQL SÍ
        // Ajusta según tu BD real
        Assert.Null(result); // Asumiendo que es case-sensitive
    }

    [Fact]
    public async Task LoginUser_WithLeadingTrailingSpacesInEmail_ShouldTrim()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "  test@example.com  ", // Espacios
            Password = "password123"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert - Tu método NO hace trim, así que debería fallar
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginUser_WithUserHavingMultipleCompanies_LoadsCompaniesCorrectly()
    {
        // Arrange - Usuario con compañías
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1),
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
        Assert.Equal(2, capturedUser.UserCompanies?.Count); // Verificar que se cargaron las compañías
    }

    [Fact]
    public async Task LoginUser_WithWhitespacePassword_ShouldBeTreatedAsValidPassword()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "   "); // Password con espacios

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "   " // Mismos espacios
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task LoginUser_WhenPasswordVerificationSucceeds_LogsInformation()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
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

        _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns(new TokenDto { Token = "test-token" });

        // Act
        await _userService.LoginUserAsync(loginDto);

        // Assert - Verificar log de información
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("User logged in") && o.ToString().Contains("test@example.com")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
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
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
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

        // Verificar log de advertencia
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Failed login attempt") && o.ToString().Contains("test@example.com")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
