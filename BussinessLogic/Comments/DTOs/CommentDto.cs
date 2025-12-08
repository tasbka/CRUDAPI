namespace BussinessLogic.Comments.DTOs;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid NoteId { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorAvatar { get; set; } = "👤";
    public Guid? ParentCommentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<CommentDto> Replies { get; set; } = new();
}

public class CreateCommentDto
{
    public Guid NoteId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? ParentCommentId { get; set; }
}

public class UpdateCommentDto
{
    public string Content { get; set; } = string.Empty;
}