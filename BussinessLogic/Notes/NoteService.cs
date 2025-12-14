using BussinessLogic.DTOs;
using BussinessLogic.Exceptions;
using DataAccess;
using DataAccess.Category;
using DataAccess.Users;

namespace BussinessLogic;

public class NoteService(INoteRepository noteRepository, IUserRepository userRepository,ICategoryRepository categoryRepository) : INoteService
{
    
    public async  Task<NoteDto> CreateAsync(Guid userId, Guid categoryId, string title, string content, CancellationToken cancellationToken = default)
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
            Title = title,         
            Content = content,      
            AuthorId = userId,
            CategoryId = categoryId, 
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };
        await noteRepository.CreateAsync(note, cancellationToken);

        user.PostCount++;

        if (user.PostCount >= 10 && user.Role == "Novice")
        {
            user.Role = "Expert";
        }
        
        
        await userRepository.UpdateAsync(user, cancellationToken);
        return MapToDto(note, user, category);
    }
    
    // ПОЛУЧЕНИЕ ПО ID
    public async Task<NoteDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var note = await noteRepository.GetByIdAsync(id, cancellationToken);
        if (note == null)
        {
            throw new ArgumentException("Note not found");
        }

        var author = await userRepository.GetByIdAsync(note.AuthorId, cancellationToken);
        var category = await categoryRepository.GetByIdAsync(note.CategoryId, cancellationToken);
        
        return MapToDto(note, author, category);
    }

    // ОБНОВЛЕНИЕ
    public async  Task<NoteDto> UpdateAsync(Guid id, string newTitle, string newText,  CancellationToken cancellationToken = default)
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
        
        var author = await userRepository.GetByIdAsync(note.AuthorId, cancellationToken);
        var category = await categoryRepository.GetByIdAsync(note.CategoryId, cancellationToken);
        
        return MapToDto(note, author, category);
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
    
    //строку с заметками пользователя
    public async Task<IEnumerable<NoteDto>> GetUserNotesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // 1. Получаем заметки пользователя
        var notes = await noteRepository.GetByUserIdAsync(userId, cancellationToken);
    
        if (!notes.Any())
        {
            return Enumerable.Empty<NoteDto>(); 
        }
        
        var result = new List<NoteDto>();
        foreach (var note in notes)
        {
            if (!note.IsActive) continue; 
            
            var author = await userRepository.GetByIdAsync(note.AuthorId, cancellationToken);
            var category = await categoryRepository.GetByIdAsync(note.CategoryId, cancellationToken);
        
            result.Add(MapToDto(note, author, category));
        }
    
        return result;
    }
    
    // ПОЛУЧЕНИЕ ВСЕХ ЗАМЕТОК
    public async Task<IEnumerable<NoteDto>> GetAllNotesAsync(CancellationToken cancellationToken = default)
    {
        var notes = await noteRepository.GetAllAsync(cancellationToken);
        
        var result = new List<NoteDto>();
        foreach (var note in notes)
        {
            if (!note.IsActive) continue;
            
            var author = await userRepository.GetByIdAsync(note.AuthorId, cancellationToken);
            var category = await categoryRepository.GetByIdAsync(note.CategoryId, cancellationToken);
            
            result.Add(MapToDto(note, author, category));
        }
        
        return result;
    }
    
    // Метод для преобразования Note в NoteDto
    private NoteDto MapToDto(Note note, User? author, Category? category)
    {
        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CategoryId = note.CategoryId,
            CategoryName = category?.Name ?? "Без категории",
            AuthorId = note.AuthorId,
            AuthorName = author?.Username ?? "Аноним",
            Created = note.Created,
            Updated = note.Updated,
            ViewCount = note.ViewCount,
            CountComments = note.CountComments,
            LikeCount = note.LikeCount,
            IsPinned = note.IsPinned,
            IsSolved = note.IsSolved
        };
    }
}