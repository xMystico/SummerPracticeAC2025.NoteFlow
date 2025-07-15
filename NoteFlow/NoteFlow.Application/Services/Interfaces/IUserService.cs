using NoteFlow.Application.Models.Common;
using NoteFlow.Application.Models.User.DTO;

namespace NoteFlow.Application.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserIdentificationDTO>> AuthenticateAsync(string username, string password);

    Task<Result<UserIdentificationDTO>> RegisterAsync(string username, string password, string email);

    Task<Result<UserDTO>> GetByIdAsync(Guid id);

    Task<Result<UserDTO>> GetByUsernameOrEmailAsync(string usernameOrEmail);

    Task<Result> VerifyEmailAsync(Guid userId, string verificationToken);
}