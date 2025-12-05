using BussinessLogic.Users;
using BussinessLogic.Users.DTOs;
using DataAccess.Users;
using DataAccess.Helpers;

namespace BusinessLogic.Users;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserDto> CreateAsync(string username, string email, string password, CancellationToken cancellationToken = default)
    {
        // Проверяем, существует ли пользователь с таким username
        var existingUser = await userRepository.GetByUsernameAsync(username, cancellationToken);
        if (existingUser != null)
        {
            throw new ArgumentException("Username already exists");
        }

        // Проверяем, существует ли пользователь с таким email
        var existingEmail = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingEmail != null)
        {
            throw new ArgumentException("Email already exists");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            throw new ArgumentException("Пароль должен содержать минимум 6 символов");
        }
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = password, // Просто сохраняем пароль как есть (на время разработки!)
            Role = "Novice",
            PostCount = 0,
            Reputation = 0,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await userRepository.CreateAsync(user, cancellationToken);
        return MapToDto(user);
    }

    
    
    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("Пользователь не найден");
        }
        return MapToDto(user);
        //return $"Username: {user.Username}, Email: {user.Email}, Created: {user.CreatedAt:yyyy-MM-dd HH:mm}";
    }
    public async Task<UserDto?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUsernameAsync(username, cancellationToken);
        
        if (user == null || !user.IsActive)
            return null;
        
        if (user.PasswordHash != password)
            return null;
            
        return MapToDto(user);
    }
    
    public async Task<UserDto> UpdateAsync(Guid id, string? newUsername, string? newEmail, string? newPassword, CancellationToken cancellationToken = default)
    {
        bool hasChanges = false;
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("Пользователь не найден");
        }

        if (!string.IsNullOrWhiteSpace(newUsername) && user.Username != newUsername)
        {
            var existingUser = await userRepository.GetByUsernameAsync(newUsername, cancellationToken);
            if (existingUser != null && existingUser.Id != id)
            {
                throw new ArgumentException("Имя пользователя уже занято");
            }
            user.Username = newUsername;
            hasChanges = true;
        }

        // Проверяем, не занят ли новый email другим пользователем
        if (!string.IsNullOrWhiteSpace(newEmail) && user.Email != newEmail)
        {
            var existingEmail = await userRepository.GetByEmailAsync(newEmail, cancellationToken);
            if (existingEmail != null && existingEmail.Id != id)
            {
                throw new ArgumentException("Email уже используется");
            }
            user.Email = newEmail;
            hasChanges = true;
        }

        // Обновление пароля
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            if (newPassword.Length < 6)
                throw new ArgumentException("Пароль должен содержать минимум 6 символов");
                
            user.PasswordHash = newPassword; // Просто сохраняем как есть
            hasChanges = true;
        }
        
        if (hasChanges)
        {
            await userRepository.UpdateAsync(user, cancellationToken);
        }
        
        return MapToDto(user);
        //await userRepository.UpdateAsync(user, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("Пользователь не найден");
        }

        await userRepository.DeleteAsync(user, cancellationToken);
    }

    public async Task<bool> UserExistsAsync(string email, string username, CancellationToken cancellationToken = default)
    {
        // Используем методы, которые уже есть в репозитории
        var usernameExists = await userRepository.UsernameExistsAsync(username, cancellationToken);
        var emailExists = await userRepository.EmailExistsAsync(email, cancellationToken);
    
        return usernameExists || emailExists;
    }
    /*
    private static string HashPassword(string password)
    {
        // Временная реализация - для продакшена используйте BCrypt
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
    
     public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.Users
            .Where(u => u.IsActive)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync(cancellationToken);
            
        return users.Select(MapToDto);
    }
    */
    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            PostCount = user.PostCount,
            Reputation = user.Reputation,
            CreatedAt = user.CreatedAt,
        };
    }
}