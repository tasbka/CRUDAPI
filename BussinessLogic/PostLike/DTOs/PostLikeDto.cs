namespace BussinessLogic.DTOs;

public class PostLikeDto
{
    public Guid Id { get; set; }
    public Guid NoteId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
