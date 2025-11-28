using DataAccess;

namespace BussinessLogic;

public interface INoteService
{
    Task CreateAsync(Guid userId, string text, CancellationToken cancellationToken = default);
    Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, string newText, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<string> GetUserNotesAsync(Guid userId, CancellationToken cancellationToken = default);
    
}