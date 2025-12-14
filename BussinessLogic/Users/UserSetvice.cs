using BussinessLogic.Users;
using BussinessLogic.Users.DTOs;
using DataAccess.Users;
using DataAccess.Helpers;

namespace BusinessLogic.Users;

public class UserService(IUserRepository userRepository, PasswordHasher passwordHasher) : IUserService
{
    public async Task<UserDto> CreateAsync(string username, string email, string password, CancellationToken cancellationToken = default)
    {
        // существует ли пользователь с таким username
        var existingUser = await userRepository.GetByUsernameAsync(username, cancellationToken);
        if (existingUser != null)
        {
            throw new ArgumentException("Username already exists");
        }

        //  существует ли пользователь с таким email
        var existingEmail = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingEmail != null)
        {
            throw new ArgumentException("Email already exists");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            throw new ArgumentException("Пароль должен содержать минимум 6 символов");
        }
        
        var passwordHash = passwordHasher.HashPassword(password); 
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
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
     
    }
    public async Task<UserDto?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUsernameAsync(username, cancellationToken);
        
        if (user == null || !user.IsActive)
            return null;
        
        if (!passwordHasher.VerifyPassword(password, user.PasswordHash))
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
                
            user.PasswordHash = newPassword;
            hasChanges = true;
        }
        
        if (hasChanges)
        {
            await userRepository.UpdateAsync(user, cancellationToken);
        }
        
        return MapToDto(user);
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
        var usernameExists = await userRepository.UsernameExistsAsync(username, cancellationToken);
        var emailExists = await userRepository.EmailExistsAsync(email, cancellationToken);
    
        return usernameExists || emailExists;
    }

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