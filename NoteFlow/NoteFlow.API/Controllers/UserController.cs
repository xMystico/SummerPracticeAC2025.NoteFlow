namespace NoteFlow.API.Controllers;

using Application.Models.User.Requests;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class UserController(IUserService userService) : BaseController
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        try
        {
            var result = await userService.RegisterAsync(model.Username, model.Password, model.Email);

            return this.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        var result = await userService.AuthenticateAsync(model.Username, model.Password);

        if (!result.IsSuccess)
            return this.Unauthorized();

        return this.Ok(result);
    }

    [HttpPost("Verify-Email")]
    public async Task<IActionResult> VerifyEmail(Guid userId, [FromBody] VerifyEmailRequestModel model)
    {
        try
        {
            var result = await userService.VerifyEmailAsync(userId, model.VerificationToken);
            return this.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await userService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            return this.NotFound($"User with id: {id} was not found");
        }

        return this.Ok(result);
    }

    [HttpGet("GetUserByUsernameOrEmail")]
    public async Task<IActionResult> GetUserByUsernameOrEmail(string usernameOrEmail)
    {
        var result = await userService.GetByUsernameOrEmailAsync(usernameOrEmail);

        if (!result.IsSuccess)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }
}