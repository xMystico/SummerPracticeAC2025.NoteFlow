using NoteFlow.Application.Models.Common;

namespace NoteFlow.Application.Models.User.DTO;

public class UserDTO : BaseDTO
{
    public string Username { get; set; }

    public string Email { get; set; }
}