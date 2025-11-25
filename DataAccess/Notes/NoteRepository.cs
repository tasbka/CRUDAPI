using Microsoft.EntityFrameworkCore;

namespace DataAccess;

internal class NoteRepository(AppContext context) : INoteRepository
{
    private INoteRepository _noteRepositoryImplementation;

    public async Task CreateAsync(Note note, CancellationToken cancellationToken = default)
    { 
        note.Created =  DateTime.UtcNow;
       await context.Notes.AddAsync(note, cancellationToken);
       await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Notes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Note>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Where(x => x.UserId == userId)
            .Include(n => n.User)
            .OrderByDescending(n => n.Created)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Note>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .Include(n => n.User)
            .OrderByDescending(n => n.Created)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
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