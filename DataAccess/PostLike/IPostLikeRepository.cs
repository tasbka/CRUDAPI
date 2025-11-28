namespace DataAccess;

public interface IPostLikeRepository
{
    Task CreateAsync(PostLike postLike, CancellationToken cancellationToken = default);
    Task<PostLike?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PostLike?> GetByNoteAndUserAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
    Task<List<PostLike>> GetByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default);
    Task<List<PostLike>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(PostLike postLike, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetLikeCountAsync(Guid noteId, CancellationToken cancellationToken = default);
}