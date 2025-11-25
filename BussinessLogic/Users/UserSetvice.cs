using BussinessLogic.Users;
using DataAccess.Users;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Users;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task CreateAsync(string username, string email, string password, CancellationToken cancellationToken = default)
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

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.CreateAsync(user, cancellationToken);
    }

    public async Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        return $"Username: {user.Username}, Email: {user.Email}, Created: {user.CreatedAt:yyyy-MM-dd HH:mm}";
    }

    public async Task UpdateAsync(Guid id, string newUsername, string newEmail, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        // Проверяем, не занят ли новый username другим пользователем
        if (user.Username != newUsername)
        {
            var existingUser = await userRepository.GetByUsernameAsync(newUsername, cancellationToken);
            if (existingUser != null)
            {
                throw new ArgumentException("Username already taken");
            }
            user.Username = newUsername;
        }

        // Проверяем, не занят ли новый email другим пользователем
        if (user.Email != newEmail)
        {
            var existingEmail = await userRepository.GetByEmailAsync(newEmail, cancellationToken);
            if (existingEmail != null)
            {
                throw new ArgumentException("Email already taken");
            }
            user.Email = newEmail;
        }

        await userRepository.UpdateAsync(user, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        await userRepository.DeleteAsync(user, cancellationToken);
    }

    private static string HashPassword(string password)
    {
        // Временная реализация - для продакшена используйте BCrypt
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}