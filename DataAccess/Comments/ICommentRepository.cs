namespace DataAccess.Comments;

public interface ICommentRepository
{
    // Получение комментариев
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Comment>> GetByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default);
    Task<List<Comment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<Comment>> GetRepliesAsync(Guid parentCommentId, CancellationToken cancellationToken = default);
    
    // Подсчеты
    Task<int> GetCountByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default);
    Task<int> GetCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    // CRUD операции
    Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken = default);
    Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default);
    
    // Проверки
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    
    // Дополнительные методы не реализ
    Task<List<Comment>> GetLatestCommentsAsync(int count, CancellationToken cancellationToken = default);
    Task<List<Comment>> SearchAsync(string searchText, CancellationToken cancellationToken = default);
    Task<bool> IsOwnerAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default);
    
    // Пагинация не реализ
    Task<(List<Comment> Comments, int TotalCount)> GetByNoteIdWithPaginationAsync(
        Guid noteId, int page, int pageSize, CancellationToken cancellationToken = default);
}