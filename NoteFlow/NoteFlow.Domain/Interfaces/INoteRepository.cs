using NoteFlow.Domain.Entities;

namespace NoteFlow.Domain.Interfaces;

public interface INoteRepository
{
    Task AddAsync(Note note);

    Task UpdateAsync(Note note);

    Task DeleteAsync(Guid id);

    Task SaveAsync();

    Task<Note?> GetByIdAsync(Guid id);

    Task<List<Note>> GetAllAsync();

    Task<List<Note>> GetByUserIdAsync(Guid userId);
}