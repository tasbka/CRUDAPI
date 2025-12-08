using System.ComponentModel.DataAnnotations;
using DataAccess.Users;

namespace DataAccess.Comments;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public Guid NoteId { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    public Guid? ParentCommentId { get; set; } 
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Навигационные свойства
    public virtual Note Note { get; set; } = null!;
    public virtual User Author { get; set; } = null!;
    public virtual Comment? ParentComment { get; set; }
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
}