namespace BussinessLogic;

public interface ICategoryService
{
    Task CreateAsync(string name, string description, int orderIndex, CancellationToken cancellationToken = default);
    Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<string> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, string newName, string newDescription, int newOrderIndex, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}