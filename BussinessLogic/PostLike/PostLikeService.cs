using DataAccess;
using DataAccess.Users;

namespace BussinessLogic;

public class PostLikeService : IPostLikeService
{
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly INoteRepository _noteRepository;
    private readonly IUserRepository _userRepository;

    public PostLikeService(IPostLikeRepository noteLikeRepository, INoteRepository noteRepository, IUserRepository userRepository)
    {
        _postLikeRepository = noteLikeRepository;
        _noteRepository = noteRepository;
        _userRepository = userRepository;
    }

    public async Task LikeNoteAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default)
    {
        // проверка на существование заметки
        var note = await _noteRepository.GetByIdAsync(noteId, cancellationToken);
        if (note == null)
        {
            throw new ArgumentException("Note not found");
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        // проверка на второй лайк от пользователя
        var existingLike = await _postLikeRepository.GetByNoteAndUserAsync(noteId, userId, cancellationToken);
        if (existingLike != null)
        {
            throw new ArgumentException("Note already liked by user");
        }

        // Создаем лайк
        var noteLike = new DataAccess.PostLike
        {
            NoteId = noteId,
            UserId = userId
        };

        await _postLikeRepository.CreateAsync(noteLike, cancellationToken);

        // ОБНОВИТЬ КОЛ-ВО ЛАЙКОВ
        note.LikeCount++;
        await _noteRepository.UpdateAsync(note, cancellationToken);
    }

    public async Task UnlikeNoteAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default)
    {
        var existingLike = await _postLikeRepository.GetByNoteAndUserAsync(noteId, userId, cancellationToken);
        if (existingLike == null)
        {
            throw new ArgumentException("Note not liked by user");
        }

        await _postLikeRepository.DeleteAsync(existingLike, cancellationToken);

        // счетчик лайков обновить 
        var note = await _noteRepository.GetByIdAsync(noteId, cancellationToken);
        if (note != null)
        {
            note.LikeCount = Math.Max(0, note.LikeCount - 1);
            await _noteRepository.UpdateAsync(note, cancellationToken);
        }
    }

    public async Task<int> GetLikeCountAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await _postLikeRepository.GetLikeCountAsync(noteId, cancellationToken);
    }

    public async Task<bool> IsNoteLikedByUserAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _postLikeRepository.ExistsAsync(noteId, userId, cancellationToken);
    }
}