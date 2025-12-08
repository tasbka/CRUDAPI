using BussinessLogic.Comments.DTOs;

namespace BussinessLogic.Comments;

public interface ICommentService
{
    Task<CommentDto> CreateCommentAsync(CreateCommentDto request, CancellationToken cancellationToken = default);
    Task<CommentDto> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentDto>> GetCommentsByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentDto>> GetRepliesAsync(Guid parentCommentId, CancellationToken cancellationToken = default);
    Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentDto request, CancellationToken cancellationToken = default);
    Task DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetCommentCountByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default);
    Task<int> GetCommentCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}