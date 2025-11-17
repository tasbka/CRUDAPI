namespace DataAccess.Users;
using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}