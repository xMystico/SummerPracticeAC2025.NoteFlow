namespace NoteFlow.Infrastructure.Repositories;

using Data;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class NoteRepository(AppDbContext context) : INoteRepository
{
    public async Task AddAsync(Note note)
    {
        await context.Notes.AddAsync(note);
    }

    public async Task UpdateAsync(Note note)
    {
        context.Notes.Update(note);
    }

    public async Task DeleteAsync(Guid id)
    {
        var note = await context.Notes.FindAsync(id);

        if (note == null)
        {
            throw new Exception("Note not found");
        }

        context.Notes.Remove(note);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<Note?> GetByIdAsync(Guid id)
    {
        return await context.Notes.FirstOrDefaultAsync(tp => tp.Id == id);
    }

    public async Task<List<Note>> GetAllAsync()
    {
        return await context.Notes.ToListAsync();
    }

    public async Task<List<Note>> GetByUserIdAsync(Guid userId)
    {
        return await context.Notes.Where(tp => tp.UserId == userId).ToListAsync();
    }
}