namespace BussinessLogic.Comments.DTOs;

public class CreateCommentRequest
{
    public Guid NoteId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? ParentCommentId { get; set; }
}