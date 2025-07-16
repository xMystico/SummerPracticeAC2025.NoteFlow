namespace NoteFlow.Application.Services;

using Models.Common;
using Models.User.DTO;
using Interfaces;
using System.Text;
using Constants;
using Mappers.User;
using Domain.Entities;
using NoteFlow.Domain.Interfaces;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Result<UserIdentificationDTO>> AuthenticateAsync(string username, string password)
    {
        var user = await userRepository.GetByUsernameOrEmailAsync(username);

        if (user == null || user.Password != this.EncryptPassword(password))
        {
            return new ErrorResult<UserIdentificationDTO>(Errors.AuthenticationFailed);
        }

        return new SuccessResult<UserIdentificationDTO>(new UserIdentificationDTO { Id = user.Id });
    }

    public async Task<Result<UserIdentificationDTO>> RegisterAsync(string username, string password, string email)
    {
        var existingUser = await userRepository.GetByUsernameOrEmailAsync(username);

        if (existingUser != null)
        {
            throw new ArgumentException(Errors.UsernameExists);
        }

        var encryptedPassword = this.EncryptPassword(password);

        var user = new User
        {
            Username = username,
            Password = encryptedPassword,
            Email = email,
            IsEmailVerified = false
        };

        var entityId = await userRepository.AddAsync(user);
        await userRepository.SaveAsync();

        return new SuccessResult<UserIdentificationDTO>(new UserIdentificationDTO { Id = entityId });
    }

    public async Task<Result> VerifyEmailAsync(Guid userId, string verificationToken)
    {

        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new ArgumentException(Errors.UserNotFound);
        }

        if (verificationToken == UserConstants.ValidToken)
        {
            user.IsEmailVerified = true;

            await userRepository.SaveAsync();

            return new SuccessResult();
        }

        throw new ArgumentException(Errors.InvalidVerificationToken);
    }

    public async Task<Result<UserDTO>> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return new ErrorResult<UserDTO>(null);
        }

        var userDto = user.ToUserDTO();

        return new SuccessResult<UserDTO>(userDto);
    }

    public async Task<Result<UserDTO>> GetByUsernameOrEmailAsync(string usernameOrEmail)
    {
        var user = await userRepository.GetByUsernameOrEmailAsync(usernameOrEmail);

        var userDto = user.ToUserDTO();

        return new SuccessResult<UserDTO>(userDto);
    }

    private string EncryptPassword(string password)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }
}