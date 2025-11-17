using DataAccess.Users;

namespace DataAccess;

public class Note
{
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}