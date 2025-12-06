

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Notes;

public class NoteRepository(AppContext context) : INoteRepository
{
    private INoteRepository _noteRepositoryImplementation;

    public async Task CreateAsync(Note note, CancellationToken cancellationToken = default)
    { 
        note.Created =  DateTime.UtcNow;
       await context.Notes.AddAsync(note, cancellationToken);
       await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
    public async Task<List<Note>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Include(n => n.Author)
            .Include(n => n.Category)
            .Where(n => n.IsActive)
            .OrderByDescending(n => n.Created)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Note>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Author)
            .Include(p => p.Category)
            .OrderByDescending(p => p.IsPinned)
            .ThenByDescending(p => p.Created)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Note>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Where(x => x.AuthorId == userId)
            .Include(n => n.Author)
            .OrderByDescending(n => n.Created)
            .ToListAsync(cancellationToken);
    }
    
    /*public async Task<List<Note>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Include(n => n.Author)
            .OrderByDescending(n => n.Created)
            .ToListAsync(cancellationToken);
    }*/
    //удалила из интерфейсв
    
    public async Task<List<Note>> GetPinnedAsync(CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Where(p => p.IsPinned)
            .Include(p => p.Author)
            .Include(p => p.Category)
            .OrderByDescending(p => p.Created)
            .ToListAsync(cancellationToken);
    }
    
    
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        note.Updated = DateTime.UtcNow;
        context.Notes.Update(note);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Note note, CancellationToken cancellationToken = default)
    {
        context.Notes.Remove(note);
        await context.SaveChangesAsync(cancellationToken);
    }
}