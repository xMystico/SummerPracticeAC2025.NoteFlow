namespace NoteFlow.Integration.Tests;

using Application.Models.Common;
using Application.Models.User.DTO;
using Application.Models.User.Requests;
using Setup;
using System.Net.Http.Json;
using Shouldly;

public class UserIntegrationTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private const string BaseUrl = "api/v1/user";

    [Fact]
    public async Task RegisterAsync_Should_ReturnSuccess_When_RegistrationIsValid()
    {
        // Arrange
        var requestModel = new RegisterRequestModel
        {
            Username = "newuser",
            Password = "Password123!",
            Email = "test@example.com"
        };

        // Act
        var response = await this._client.PostAsJsonAsync($"{BaseUrl}/register", requestModel);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SuccessResult>();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task AuthenticateAsync_Should_ReturnSuccess_When_CredentialsAreValid()
    {
        // Arrange
        var registerModel = new RegisterRequestModel
        {
            Username = "testuser",
            Password = "Password123!",
            Email = "test@example.com"
        };
        await this._client.PostAsJsonAsync($"{BaseUrl}/register", registerModel);

        var authModel = new LoginRequestModel
        {
            Username = "testuser",
            Password = "Password123!"
        };

        // Act
        var response = await this._client.PostAsJsonAsync($"{BaseUrl}/login", authModel);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SuccessResult<UserIdentificationDTO>>();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task GetUserByIdAsync_Should_ReturnUser_When_UserExists()
    {
        // Arrange
        var registerModel = new RegisterRequestModel
        {
            Username = "testuser",
            Password = "Password123!",
            Email = "test@example.com"
        };
        var registerResponse = await this._client.PostAsJsonAsync($"{BaseUrl}/register", registerModel);
        registerResponse.EnsureSuccessStatusCode();

        var user = await registerResponse.Content.ReadFromJsonAsync<SuccessResult<UserIdentificationDTO>>();

        // Act
        var response = await this._client.GetAsync($"{BaseUrl}/{user.Data.Id}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SuccessResult<UserDTO>>();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Data.Username.ShouldBe("testuser");
    }
}