namespace DataAccess;

public interface  INoteRepository
{
    Task CreateAsync(Note note, CancellationToken cancellationToken = default);
    Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Note>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default); // Добавьте этот метод
    Task<List<Note>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task DeleteAsync(Note note, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}