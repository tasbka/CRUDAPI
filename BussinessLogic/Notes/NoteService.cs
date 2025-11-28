using BussinessLogic.Exceptions;
using DataAccess;
using DataAccess.Users;

namespace BussinessLogic;

internal class NoteService(INoteRepository noteRepository, IUserRepository userRepository) : INoteService
{
    
    public async Task CreateAsync(Guid userId, string text, CancellationToken cancellationToken = default)
    {
        
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new CrudApiNotFoundException<User>();
        }

        var note = new Note
        {
            Content = text,
            AuthorId = userId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };

        await noteRepository.CreateAsync(note, cancellationToken);
    }

    public async Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var note = await noteRepository.GetByIdAsync(id, cancellationToken);
        if (note == null)
        {
            throw new ArgumentException("Note not found");
        }

        return $"Note: {note.Id}, User: {note.Author?.Username}, Created: {note.Created}";
    }

    public async Task UpdateAsync(Guid id, string newText, CancellationToken cancellationToken = default)
    {
        var note = await noteRepository.GetByIdAsync(id, cancellationToken);
        if (note == null)
        {
            throw new ArgumentException("Note not found");
        }
        note.Content = newText;
        note.Updated = DateTime.UtcNow;
        
        await noteRepository.UpdateAsync(note, cancellationToken);
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var note = await noteRepository.GetByIdAsync(id, cancellationToken);
        if (note == null)
        {
            throw new ArgumentException("Note not found");
        }
        await noteRepository.DeleteAsync(note, cancellationToken);
    }
    
    public async Task<string> GetUserNotesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notes = await noteRepository.GetByUserIdAsync(userId, cancellationToken);
        if (!notes.Any())
        {
            return "No notes found for this user";
        }

        var result = $"User {userId} notes:\n";
        foreach (var note in notes)
        {
            result += $"- {note.Id} (Created: {note.Created})\n";
        }

        return result;
    }
}