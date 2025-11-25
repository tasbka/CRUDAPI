namespace WebApi.Models;

public class AddNoteRequest
{
    public Guid UserId { get; set; }
    public string Text { get; set; } = string.Empty;
}