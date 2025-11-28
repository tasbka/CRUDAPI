using Microsoft.EntityFrameworkCore;

namespace DataAccess.Category;

public class  CategoryRepository(AppContext context) : ICategoryRepository
{
    public async Task CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        category.CreatedAt = DateTime.UtcNow;
        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .OrderBy(c => c.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<int> GetNoteCountAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await context.Notes
            .CountAsync(n => n.CategoryId == categoryId && n.IsActive, cancellationToken);
    }
}