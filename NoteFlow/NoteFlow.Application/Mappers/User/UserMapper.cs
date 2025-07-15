namespace NoteFlow.Application.Mappers.User;

using NoteFlow.Application.Models.User.DTO;

public static class UserMapper
{
    public static UserDTO ToUserDTO(this Domain.Entities.User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }
}