using System.ComponentModel.DataAnnotations;

namespace DataAccess.Category;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    public int PostCount { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int OrderIndex { get; set; } = 0;
    
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}