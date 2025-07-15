using NoteFlow.Domain.Entities;

namespace NoteFlow.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);

    Task<Guid> AddAsync(User user);

    Task SaveAsync();
}