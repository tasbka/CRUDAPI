using DataAccess;
using DataAccess.Category;

namespace BussinessLogic;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly INoteRepository _noteRepository;

    public CategoryService(ICategoryRepository categoryRepository, INoteRepository noteRepository)
    {
        _categoryRepository = categoryRepository;
        _noteRepository = noteRepository;
    }

    public async Task CreateAsync(string name, string description, int orderIndex, CancellationToken cancellationToken = default)
    {
        // есть ли такая уже
        var existingCategory = await _categoryRepository.GetByNameAsync(name, cancellationToken);
        if (existingCategory != null)
        {
            throw new ArgumentException("Category with this name already exists");
        }

        var category = new Category
        {
            Name = name,
            Description = description,
            OrderIndex = orderIndex
        };

        await _categoryRepository.CreateAsync(category, cancellationToken);
    }

    public async Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            throw new ArgumentException("Category not found");
        }

        var noteCount = await _categoryRepository.GetNoteCountAsync(id, cancellationToken);

        return $"Category: {category.Name}, Description: {category.Description}, " +
               $"Notes: {noteCount}, Order: {category.OrderIndex}";
    }

    public async Task<string> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        
        if (!categories.Any())
        {
            return "No categories found";
        }

        var result = "Categories:\n";
        foreach (var category in categories)
        {
            var noteCount = await _categoryRepository.GetNoteCountAsync(category.Id, cancellationToken);
            result += $"- {category.Name}: {noteCount} notes (Order: {category.OrderIndex})\n";
        }

        return result;
    }

    public async Task UpdateAsync(Guid id, string newName, string newDescription, int newOrderIndex, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            throw new ArgumentException("Category not found");
        }

        // занято имя хз зач
        if (category.Name != newName)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(newName, cancellationToken);
            if (existingCategory != null)
            {
                throw new ArgumentException("Category name already taken");
            }
        }

        category.Name = newName;
        category.Description = newDescription;
        category.OrderIndex = newOrderIndex;

        await _categoryRepository.UpdateAsync(category, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            throw new ArgumentException("Category not found");
        }

        // число заметок под этой категорией
        var noteCount = await _categoryRepository.GetNoteCountAsync(id, cancellationToken);
        if (noteCount > 0)
        {
            throw new InvalidOperationException($"Cannot delete category with {noteCount} notes. Move or delete notes first.");
        }

        await _categoryRepository.DeleteAsync(category, cancellationToken);
    }
}