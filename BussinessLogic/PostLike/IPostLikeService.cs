using BussinessLogic.DTOs;

namespace BussinessLogic;

public interface IPostLikeService
{
    Task<LikeResponseDto> LikeNoteAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
    Task<LikeResponseDto> UnlikeNoteAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
    Task<LikeResponseDto> ToggleLikeAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetLikeCountAsync(Guid noteId, CancellationToken cancellationToken = default);
    Task<bool> IsNoteLikedByUserAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);
}