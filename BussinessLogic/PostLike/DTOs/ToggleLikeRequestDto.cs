namespace BussinessLogic.DTOs;

public class ToggleLikeRequestDto
{
    public Guid NoteId { get; set; }
    public Guid UserId { get; set; }
}