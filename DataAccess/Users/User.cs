using System.ComponentModel.DataAnnotations;
namespace DataAccess.Users;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(20)]
    public string Role { get; set; } = "Novice";
    
    public int PostCount { get; set; } = 0;
    
    public int Reputation { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
