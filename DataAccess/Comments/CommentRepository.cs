using Microsoft.EntityFrameworkCore;

namespace DataAccess.Comments;

public class CommentRepository : ICommentRepository
{
 private readonly AppContext _context;
    
    public CommentRepository(AppContext context)
    {
        _context = context;
    }
    
    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Include(c => c.Author)
            .Include(c => c.Note)
            .Include(c => c.ParentComment)
            .Include(c => c.Replies)
                .ThenInclude(r => r.Author)
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive, cancellationToken);
    }
    
    public async Task<List<Comment>> GetByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.NoteId == noteId && c.IsActive && c.ParentCommentId == null)
            .Include(c => c.Author)
            .Include(c => c.Replies)
                .ThenInclude(r => r.Author)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Comment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.AuthorId == userId && c.IsActive)
            .Include(c => c.Note)
            .Include(c => c.Author)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Comment>> GetRepliesAsync(Guid parentCommentId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.ParentCommentId == parentCommentId && c.IsActive)
            .Include(c => c.Author)
            .Include(c => c.ParentComment)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<int> GetCountByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .CountAsync(c => c.NoteId == noteId && c.IsActive, cancellationToken);
    }
    
    public async Task<int> GetCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .CountAsync(c => c.AuthorId == userId && c.IsActive, cancellationToken);
    }
    
    public async Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        comment.CreatedAt = DateTime.UtcNow;
        comment.IsActive = true;
        
        await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return comment;
    }
    
    public async Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        comment.UpdatedAt = DateTime.UtcNow;
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return comment;
    }
    
    public async Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        comment.IsActive = false;
        comment.UpdatedAt = DateTime.UtcNow;
        await UpdateAsync(comment, cancellationToken);
    }
    
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .AnyAsync(c => c.Id == id && c.IsActive, cancellationToken);
    }
    
    public async Task<List<Comment>> GetLatestCommentsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.IsActive && c.ParentCommentId == null) // Только корневые комментарии
            .Include(c => c.Author)
            .Include(c => c.Note)
            .OrderByDescending(c => c.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Comment>> SearchAsync(string searchText, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return new List<Comment>();
        
        var search = searchText.ToLower();
        return await _context.Comments
            .Where(c => c.IsActive && 
                   (c.Content.ToLower().Contains(search) || 
                    c.Author.Username.ToLower().Contains(search)))
            .Include(c => c.Author)
            .Include(c => c.Note)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    // Доп. методы
    // Дополнительный метод: получить все комментарии темы (включая ответы)
    public async Task<List<Comment>> GetAllByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.NoteId == noteId && c.IsActive)
            .Include(c => c.Author)
            .Include(c => c.ParentComment)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    // Дополнительный метод: проверить, принадлежит ли комментарий пользователю
    public async Task<bool> IsOwnerAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .AnyAsync(c => c.Id == commentId && c.AuthorId == userId && c.IsActive, cancellationToken);
    }
    
    // Дополнительный метод: получить комментарии с пагинацией
    public async Task<(List<Comment> Comments, int TotalCount)> GetByNoteIdWithPaginationAsync(
        Guid noteId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Comments
            .Where(c => c.NoteId == noteId && c.IsActive && c.ParentCommentId == null)
            .Include(c => c.Author)
            .Include(c => c.Replies)
                .ThenInclude(r => r.Author);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var comments = await query
            .OrderBy(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (comments, totalCount);
    }
}