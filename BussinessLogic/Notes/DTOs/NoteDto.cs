namespace BussinessLogic.DTOs;

public class NoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsPinned { get; set; }
    public bool IsSolved { get; set; }
}