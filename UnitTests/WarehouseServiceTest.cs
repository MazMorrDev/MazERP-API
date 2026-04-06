using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using MazErpAPI.Context;
using MazErpAPI.DTOs.Company;
using MazErpAPI.DTOs.User;
using MazErpAPI.Enums;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;

namespace MazErpAPI.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<DbSet<User>> _mockUserSet;
        private readonly Mock<DbSet<UserCompany>> _mockUserCompanySet;
        private readonly Mock<ICompanyService> _mockCompanyService;
        private readonly Mock<UserMapper> _mockUserMapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _mockUserSet = new Mock<DbSet<User>>();
            _mockUserCompanySet = new Mock<DbSet<UserCompany>>();
            _mockCompanyService = new Mock<ICompanyService>();
            _mockUserMapper = new Mock<UserMapper>();

            _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);
            _mockContext.Setup(c => c.UserCompanies).Returns(_mockUserCompanySet.Object);

            _userService = new UserService(
                _mockContext.Object,
                _mockUserMapper.Object,
                _mockCompanyService.Object,
                Mock.Of<ILogger<UserService>>()
            );
        }

        #region CreateUserAsync Tests

        [Fact]
        public async Task CreateUserAsync_ValidUser_ShouldReturnUserDto()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+1234567890"
            };

            var user = new User { Id = 1, Email = "test@example.com", FirstName = "John", LastName = "Doe", IsActive = true };
            var expectedUserDto = new UserDto { Id = 1, Email = "test@example.com", FirstName = "John", LastName = "Doe" };

            _mockUserMapper.Setup(m => m.MapCreateDtoToModel(createUserDto)).Returns(user);
            _mockUserMapper.Setup(m => m.MapToDto(user)).Returns(expectedUserDto);

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserDto.Email, result.Email);
            _mockUserSet.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_UserWithExistingEmail_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Email = "existing@example.com" };
            var existingUsers = new List<User> { new() { Email = "existing@example.com", IsActive = true } }.AsQueryable();
            var mockUserSet = CreateMockDbSet(existingUsers);
            _mockContext.Setup(c => c.Users).Returns(mockUserSet.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _userService.CreateUserAsync(createUserDto)
            );
            Assert.Equal($"User with email {createUserDto.Email} already exists", exception.Message);
        }

        #endregion

        #region GetUserByIdAsync Tests

        [Fact]
        public async Task GetUserByIdAsync_ExistingActiveUser_ShouldReturnUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { Id = userId, Email = "test@example.com", IsActive = true };
            _mockUserSet.Setup(x => x.FindAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingOrInactiveUser_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var userId = 999;
            _mockUserSet.Setup(x => x.FindAsync(userId)).ReturnsAsync(null as User);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetUserByIdAsync(userId));
        }

        #endregion

        #region UpdateUserAsync Tests

        [Fact]
        public async Task UpdateUserAsync_ValidUpdate_ShouldReturnUpdatedUserDto()
        {
            // Arrange
            var userId = 1;
            var updateUserDto = new UpdateUserDto { FirstName = "UpdatedFirstName", LastName = "UpdatedLastName" };
            var existingUser = new User { Id = userId, Email = "test@example.com", IsActive = true };
            var expectedUserDto = new UserDto { Id = userId, FirstName = "UpdatedFirstName", LastName = "UpdatedLastName" };

            _mockUserSet.Setup(x => x.FindAsync(userId)).ReturnsAsync(existingUser);
            _mockUserMapper.Setup(m => m.MapToDto(It.IsAny<User>())).Returns(expectedUserDto);

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.Equal("UpdatedFirstName", result.FirstName);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        #endregion

        #region DeleteUserAsync Tests

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_ShouldSoftDeleteUser()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User { Id = userId, IsActive = true };
            _mockUserSet.Setup(x => x.FindAsync(userId)).ReturnsAsync(existingUser);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(existingUser.IsActive);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        #endregion

        #region AddUserToCompanyAsync Tests

        [Fact]
        public async Task AddUserToCompanyAsync_ValidData_ShouldAddUserToCompany()
        {
            // Arrange
            var addUserDto = new AddUserToCompanyDto { UserId = 1, Role = UserCompanyRole.Admin };
            var companyId = 1;
            var user = new User { Id = 1, IsActive = true };
            var company = new Company { Id = companyId, IsActive = true };

            _mockUserSet.Setup(x => x.FindAsync(1)).ReturnsAsync(user);
            _mockCompanyService.Setup(x => x.GetCompanyByIdAsync(companyId)).ReturnsAsync(company);

            // Act
            await _userService.AddUserToCompanyAsync(1, companyId, addUserDto);

            // Assert
            _mockUserCompanySet.Verify(x => x.Add(It.IsAny<UserCompany>()), Times.Once);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AddUserToCompanyAsync_UserAlreadyInCompany_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var addUserDto = new AddUserToCompanyDto { UserId = 1, Role = UserCompanyRole.Admin };
            var companyId = 1;
            var user = new User { Id = 1, IsActive = true };
            var company = new Company { Id = companyId, IsActive = true };

            var existingUserCompanies = new List<UserCompany> { new() { UserId = 1, CompanyId = companyId, IsActive = true } }.AsQueryable();
            var mockUserCompanySet = CreateMockDbSet(existingUserCompanies);

            _mockContext.Setup(c => c.UserCompanies).Returns(mockUserCompanySet.Object);
            _mockUserSet.Setup(x => x.FindAsync(1)).ReturnsAsync(user);
            _mockCompanyService.Setup(x => x.GetCompanyByIdAsync(companyId)).ReturnsAsync(company);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _userService.AddUserToCompanyAsync(1, companyId, addUserDto)
            );
        }

        #endregion

        #region GetUserCompaniesAsync Tests

        [Fact]
        public async Task GetUserCompaniesAsync_UserWithCompanies_ShouldReturnUserCompanyDtos()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, IsActive = true };

            var userCompanies = new List<UserCompany>
            {
                new() { UserId = userId, CompanyId = 1, Role = UserCompanyRole.Admin, Company = new Company { Id = 1, Name = "Company 1" }, IsActive = true },
                new() { UserId = userId, CompanyId = 2, Role = UserCompanyRole.Member, Company = new Company { Id = 2, Name = "Company 2" }, IsActive = true }
            }.AsQueryable();

            var mockUserCompanySet = CreateMockDbSet(userCompanies);
            _mockContext.Setup(c => c.UserCompanies).Returns(mockUserCompanySet.Object);
            _mockUserSet.Setup(x => x.FindAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserCompaniesAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
        }

        #endregion

        #region RemoveUserFromCompanyAsync Tests

        [Fact]
        public async Task RemoveUserFromCompanyAsync_ValidAssociation_ShouldSoftDeleteAssociation()
        {
            // Arrange
            var deleteDto = new DeleteUserCompanyDto { UserId = 1, CompanyId = 1 };
            var userCompany = new UserCompany { UserId = 1, CompanyId = 1, IsActive = true };
            var queryable = new List<UserCompany> { userCompany }.AsQueryable();
            var mockUserCompanySet = CreateMockDbSet(queryable);
            _mockContext.Setup(c => c.UserCompanies).Returns(mockUserCompanySet.Object);

            // Act
            await _userService.RemoveUserFromCompanyAsync(deleteDto);

            // Assert
            Assert.False(userCompany.IsActive);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        #endregion

        #region Helper Methods

        private static Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        #endregion
    }
}