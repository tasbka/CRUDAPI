using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class AddUserRequest
{

    [Required(ErrorMessage = "Имя пользователя обязательно")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Имя пользователя должно быть от 3 до 50 символов")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Имя пользователя может содержать только буквы, цифры и подчеркивания")]
    public string Username { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    public string Password { get; set; } = string.Empty;
}