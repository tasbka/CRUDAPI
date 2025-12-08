using System.ComponentModel.DataAnnotations;
using DataAccess.Comments;
using DataAccess.Users;

namespace DataAccess;

public class Note
{
    public Guid Id { get; set; }
    
    [ MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [ MaxLength(5000)]
    public string Content { get; set; } = string.Empty; 
    
    public Guid CategoryId { get; set; }
    
    public Guid AuthorId { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime? Updated { get; set; }
    
    public bool IsPinned { get; set; } = false;
    
    public bool IsSolved { get; set; } = false;
    
    public int ViewCount { get; set; } = 0;
    
    public int LikeCount { get; set; } = 0;
    
    public bool IsActive { get; set; } = true;
    
    public virtual Category.Category Category { get; set; } = null!;
    public virtual User Author { get; set; } = null!;
    public virtual ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}