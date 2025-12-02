using DataAccess.Users;

namespace DataAccess;

public class PostLike
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid NoteId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Note Note { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}