namespace NoteFlow.Infrastructure.Repositories;

using Data;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
    }

    public async Task<Guid> AddAsync(User user)
    {
        var addedUser = await context.Users.AddAsync(user);

        return addedUser.Entity.Id;
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}