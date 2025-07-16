namespace NoteFlow.Application.Tests.Services;

using Constants;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Models.Common;
using Models.User.DTO;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;


public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        this._userRepository = Substitute.For<IUserRepository>();
        this._userService = new UserService(this._userRepository);
    }

    [Fact]
    public async Task AuthenticateAsync_Should_ReturnSuccess_When_CredentialsAreValid()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var encryptedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        var user = new User { Username = username, Password = encryptedPassword };

        this._userRepository.GetByUsernameOrEmailAsync(username)!.Returns(Task.FromResult(user));

        // Act
        var result = await this._userService.AuthenticateAsync(username, password);

        // Assert
        result.ShouldBeOfType<SuccessResult<UserIdentificationDTO>>();
    }

    [Fact]
    public async Task AuthenticateAsync_Should_ReturnError_When_UserNotFound()
    {
        // Arrange
        var username = "nonexistentuser";
        var password = "password123";

        this._userRepository.GetByUsernameOrEmailAsync(username).ReturnsNull();

        // Act
        var result = await this._userService.AuthenticateAsync(username, password);

        // Assert
        result.ShouldBeOfType<ErrorResult<UserIdentificationDTO>>();
        result.Message.ShouldBe(Errors.AuthenticationFailed);
    }

    [Fact]
    public async Task AuthenticateAsync_Should_ReturnError_When_PasswordIsIncorrect()
    {
        // Arrange
        var username = "testuser";
        var password = "wrongpassword";
        var encryptedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("correctpassword"));
        var user = new User { Username = username, Password = encryptedPassword };

        this._userRepository.GetByUsernameOrEmailAsync(username)!.Returns(Task.FromResult(user));

        // Act
        var result = await this._userService.AuthenticateAsync(username, password);

        // Assert
        result.ShouldBeOfType<ErrorResult<UserIdentificationDTO>>();
        result.Message.ShouldBe(Errors.AuthenticationFailed);
    }

    [Fact]
    public async Task RegisterAsync_Should_ThrowException_When_UsernameExists()
    {
        // Arrange
        var username = "existinguser";
        var password = "password123";
        var email = "test@example.com";
        var existingUser = new User { Username = username };

        this._userRepository.GetByUsernameOrEmailAsync(username)!.Returns(Task.FromResult(existingUser));

        // Act
        Func<Task> act = async () => await this._userService.RegisterAsync(username, password, email);

        // Assert
        await act.ShouldThrowAsync<Exception>(Errors.UsernameExists);
    }

    [Fact]
    public async Task RegisterAsync_Should_ReturnSuccess_When_RegistrationIsValid()
    {
        // Arrange
        var username = "newuser";
        var password = "password123";
        var email = "test@example.com";

        this._userRepository.GetByUsernameOrEmailAsync(username).ReturnsNull();

        // Act
        var result = await this._userService.RegisterAsync(username, password, email);

        // Assert
        result.ShouldBeOfType<SuccessResult<UserIdentificationDTO>>();
        await this._userRepository.Received(1).AddAsync(Arg.Any<User>());
        await this._userRepository.Received(1).SaveAsync();
    }

    //TODO tests for VerifyEmail and GetUserByIdAsync
}