using BussinessLogic.Users.DTOs;

namespace BussinessLogic.Users;

public interface IUserService
{
    Task<UserDto> CreateAsync(string username, string email, string password, CancellationToken cancellationToken = default);
   
    Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserDto?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<bool> UserExistsAsync(string email, string username, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateAsync(Guid id, string? newUsername, string? newEmail, string? password, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}