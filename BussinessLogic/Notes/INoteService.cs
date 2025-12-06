using BussinessLogic.DTOs;

namespace BussinessLogic;

public interface INoteService
{
    Task<NoteDto> CreateAsync(Guid userId, Guid categoryId, string title, string content, CancellationToken cancellationToken = default);
    Task<NoteDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NoteDto> UpdateAsync(Guid id, string newTitle, string newText,  CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<NoteDto>> GetUserNotesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NoteDto>> GetAllNotesAsync(CancellationToken cancellationToken = default);
}