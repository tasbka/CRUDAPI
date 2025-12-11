using BussinessLogic.Comments.DTOs;
using DataAccess;
using DataAccess.Comments;
using DataAccess.Users;

namespace BussinessLogic.Comments;

public class CommentService  : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly INoteRepository _noteRepository;
    private readonly IUserRepository _userRepository;
    
    public CommentService(
        ICommentRepository commentRepository,
        INoteRepository noteRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _noteRepository = noteRepository;
        _userRepository = userRepository;
    }
    
    public async Task<CommentDto> CreateCommentAsync(CreateCommentDto request, CancellationToken cancellationToken = default)
    {
        var note = await _noteRepository.GetByIdAsync(request.NoteId, cancellationToken);
        if (note == null)
            throw new ArgumentException("Тема не найдена");
        
        var author = await _userRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author == null)
            throw new ArgumentException("Пользователь не найден");
        
        if (request.ParentCommentId.HasValue)
        {
            var parentComment = await _commentRepository.GetByIdAsync(request.ParentCommentId.Value, cancellationToken);
            if (parentComment == null)
                throw new ArgumentException("Родительский комментарий не найден");
            
            if (parentComment.NoteId != request.NoteId)
                throw new ArgumentException("Родительский комментарий принадлежит другой теме");
        }
        
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length < 2)
            throw new ArgumentException("Комментарий должен содержать минимум 2 символа");
        
        var comment = new Comment
        {
            NoteId = request.NoteId,
            AuthorId = request.AuthorId,
            ParentCommentId = request.ParentCommentId,
            Content = request.Content.Trim(),
            IsActive = true
        };
        
        var createdComment = await _commentRepository.CreateAsync(comment, cancellationToken);
        
        author.PostCount++;
        note.CountComments++;
        
        UpdateUserRole(author);
        
        await _userRepository.UpdateAsync(author, cancellationToken);
        
        return MapToDto(createdComment, author);
        
    }
    
    public async Task<CommentDto> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        if (comment == null)
            throw new ArgumentException("Комментарий не найден");
        
        return MapToDto(comment, comment.Author);
    }
    
    public async Task<IEnumerable<CommentDto>> GetCommentsByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.GetByNoteIdAsync(noteId, cancellationToken);
        return comments.Select(c => MapToDtoWithReplies(c));
    }
    
    public async Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.GetByUserIdAsync(userId, cancellationToken);
        return comments.Select(c => MapToDto(c, c.Author));
    }
    
    public async Task<IEnumerable<CommentDto>> GetRepliesAsync(Guid parentCommentId, CancellationToken cancellationToken = default)
    {
        var replies = await _commentRepository.GetRepliesAsync(parentCommentId, cancellationToken);
        return replies.Select(r => MapToDto(r, r.Author));
    }
    
    public async Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentDto request, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        if (comment == null)
            throw new ArgumentException("Комментарий не найден");
        
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length < 2)
            throw new ArgumentException("Комментарий должен содержать минимум 2 символа");
        
        comment.Content = request.Content.Trim();
        
        var updatedComment = await _commentRepository.UpdateAsync(comment, cancellationToken);
        
        return MapToDto(updatedComment, updatedComment.Author);
    }
    
    public async Task DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        if (comment == null)
            throw new ArgumentException("Комментарий не найден");
        
        var note = await _noteRepository.GetByIdAsync(comment.NoteId, cancellationToken);
        if (note != null)
        {
            note.CountComments = Math.Max(0, note.CountComments - 1);
            await _noteRepository.UpdateAsync(note, cancellationToken);
        }
        
        var author = await _userRepository.GetByIdAsync(comment.AuthorId, cancellationToken);
        if (author != null)
        {
            author.PostCount = Math.Max(0, author.PostCount - 1);
            await _userRepository.UpdateAsync(author, cancellationToken);
        }
    
        await _commentRepository.DeleteAsync(comment, cancellationToken);
    }
    
    public async Task<int> GetCommentCountByNoteIdAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await _commentRepository.GetCountByNoteIdAsync(noteId, cancellationToken);
    }
    
    public async Task<int> GetCommentCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _commentRepository.GetCountByUserIdAsync(userId, cancellationToken);
    }
    
    public async Task<IEnumerable<CommentDto>> GetLatestCommentsAsync(int count, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.GetLatestCommentsAsync(count, cancellationToken);
        return comments.Select(c => MapToDto(c, c.Author));
    }
    
    public async Task<IEnumerable<CommentDto>> SearchCommentsAsync(string searchText, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.SearchAsync(searchText, cancellationToken);
        return comments.Select(c => MapToDto(c, c.Author));
    }
    
    public async Task<bool> IsCommentOwnerAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken);
        if (comment == null) return false;
        
        return comment.AuthorId == userId;
    }
    
    // Вспомогательные методы
    
    private CommentDto MapToDto(Comment comment, User author)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            NoteId = comment.NoteId,
            AuthorId = comment.AuthorId,
            AuthorName = author?.Username ?? "Аноним",
            AuthorAvatar = GetAvatarByRole(author?.Role ?? "Novice"),
            ParentCommentId = comment.ParentCommentId,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            Replies = new List<CommentDto>()
        };
    }
    
    private CommentDto MapToDtoWithReplies(Comment comment)
    {
        var dto = MapToDto(comment, comment.Author);
        
        // Добавляем ответы если есть
        if (comment.Replies != null && comment.Replies.Any())
        {
            foreach (var reply in comment.Replies.OrderBy(r => r.CreatedAt))
            {
                dto.Replies.Add(MapToDto(reply, reply.Author));
            }
        }
        
        return dto;
    }
    
    private string GetAvatarByRole(string role)
    {
        return role switch
        {
            "Admin" or "Админ" => "👑",
            "Мудрец" => "🧙‍♂️",
            "Expert" => "🎓",
            _ => "👤"
        };
    }
    
    private void UpdateUserRole(User user)
    {
        if (user.PostCount >= 50 && user.Role == "Expert")
        {
            user.Role = "Мудрец";
        }
        else if (user.PostCount >= 10 && user.Role == "Novice")
        {
            user.Role = "Expert";
        }
    }
}