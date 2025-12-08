using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class CreateCommentRequest
{
    [Required(ErrorMessage = "ID темы обязателен")]
    public Guid NoteId { get; set; }
    
    [Required(ErrorMessage = "ID автора обязателен")]
    public Guid AuthorId { get; set; }
    
    [Required(ErrorMessage = "Текст комментария обязателен")]
    [MinLength(2, ErrorMessage = "Комментарий должен содержать минимум 2 символа")]
    [MaxLength(5000, ErrorMessage = "Комментарий не должен превышать 5000 символов")]
    public string Content { get; set; } = string.Empty;
    
    public Guid? ParentCommentId { get; set; }
}

public class UpdateCommentRequest
{
    [Required(ErrorMessage = "Текст комментария обязателен")]
    [MinLength(2, ErrorMessage = "Комментарий должен содержать минимум 2 символа")]
    [MaxLength(5000, ErrorMessage = "Комментарий не должен превышать 5000 символов")]
    public string Content { get; set; } = string.Empty;
}