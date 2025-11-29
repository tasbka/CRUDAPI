namespace BussinessLogic;

public interface IPostLikeService
{
    Task LikeNoteAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
    Task UnlikeNoteAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetLikeCountAsync(Guid noteId, CancellationToken cancellationToken = default);
    Task<bool> IsNoteLikedByUserAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
}