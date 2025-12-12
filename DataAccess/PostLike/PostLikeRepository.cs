using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class PostLikeRepository(AppContext context) : IPostLikeRepository
{
    public async Task CreateAsync(PostLike noteLike, CancellationToken cancellationToken = default)
    {
        noteLike.CreatedAt = DateTime.UtcNow;
        await context.PostLikes.AddAsync(noteLike, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PostLike?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.PostLikes
            .Include(pl => pl.User)
            .Include(pl => pl.Note)
            .FirstOrDefaultAsync(pl => pl.Id == id, cancellationToken);
    }

    public async Task<PostLike?> GetByNoteAndUserAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.PostLikes
            .Include(pl => pl.User)
            .FirstOrDefaultAsync(nl => nl.NoteId == noteId && nl.UserId == userId, cancellationToken);
    }

    public async Task<List<PostLike>> GetByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await context.PostLikes
            .Where(nl => nl.NoteId == noteId)
            .Include(nl => nl.User)
            .OrderByDescending(pl => pl.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PostLike>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.PostLikes
            .Where(nl => nl.UserId == userId)
            .Include(nl => nl.Note)
            .OrderByDescending(pl => pl.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(PostLike noteLike, CancellationToken cancellationToken = default)
    {
        context.PostLikes.Remove(noteLike);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.PostLikes
            .AnyAsync(nl => nl.NoteId == noteId && nl.UserId == userId, cancellationToken);
    }

    public async Task<int> GetLikeCountAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await context.PostLikes
            .CountAsync(nl => nl.NoteId == noteId, cancellationToken);
    }
}