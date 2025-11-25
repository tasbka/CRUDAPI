using DataAccess;

namespace BussinessLogic;

public interface INoteService
{
    Task CreateAsync(Guid userId, string text, CancellationToken cancellationToken = default);
    Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, string newText, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<string> GetUserNotesAsync(Guid userId, CancellationToken cancellationToken = default);
    
}