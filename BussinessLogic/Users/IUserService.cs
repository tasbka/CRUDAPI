namespace BussinessLogic.Users;

public interface IUserService
{
    Task CreateAsync(string username, string email, string password, CancellationToken cancellationToken = default);
   
    Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
   
    Task UpdateAsync(Guid id, string newUsername, string newEmail, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}