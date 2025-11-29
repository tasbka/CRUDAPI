using BussinessLogic.Exceptions;
using DataAccess;
using DataAccess.Category;
using DataAccess.Users;

namespace BussinessLogic;

public class NoteService(INoteRepository noteRepository, IUserRepository userRepository,ICategoryRepository categoryRepository) : INoteService
{
    
    public async Task CreateAsync(Guid userId, Guid categoryId, string title, string content, CancellationToken cancellationToken = default)
    {
        // Проверка на существование пользователя
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new CrudApiNotFoundException<User>();
        }

        // Проверка на существование категории
        var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category == null)
        {
            throw new CrudApiNotFoundException<Category>();
        }

        var note = new Note
        {
            Title = title,           // Добавлен заголовок
            Content = content,       // Контент
            AuthorId = userId,
            CategoryId = categoryId, // Добавлена категория
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };
        await noteRepository.CreateAsync(note, cancellationToken);

        // Обновляем счетчик постов пользователя 
        user.PostCount++;
    
        // Автоматическое повышение до Expert при достижении 10 постов
        if (user.PostCount >= 10 && user.Role == "Novice")
        {
            user.Role = "Expert";
        }
    
        await userRepository.UpdateAsync(user, cancellationToken);
    }
    
    public async Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var note = await noteRepository.GetByIdAsync(id, cancellationToken);
        if (note == null)
        {
            throw new ArgumentException("Note not found");
        }

        return $"Title: {note.Title}\nContent: {note.Content}"; 
    }

    public async Task UpdateAsync(Guid id, string newTitle, string newText,  CancellationToken cancellationToken = default)
    {
        var note = await noteRepository.GetByIdAsync(id, cancellationToken);
        if (note == null)
        {
            throw new ArgumentException("Note not found");
        }
        note.Title = newTitle;
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