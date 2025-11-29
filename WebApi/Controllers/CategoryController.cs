using BussinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    // СОЗДАТЬ категорию
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        await categoryService.CreateAsync(request.Name, request.Description, request.OrderIndex);
        return Ok();
    }

    // ПОЛУЧИТЬ категорию по ID
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryAsync([FromRoute] Guid id)
    {
        var result = await categoryService.GetByIdAsync(id);
        return Ok(result);
    }

    // ПОЛУЧИТЬ все категории
    [HttpGet]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        var result = await categoryService.GetAllAsync();
        return Ok(result);
    }

    // ОБНОВИТЬ категорию
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategoryAsync([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request)
    {
        await categoryService.UpdateAsync(id, request.Name, request.Description, request.OrderIndex);
        return NoContent();
    }

    // УДАЛИТЬ категорию
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategoryAsync([FromRoute] Guid id)
    {
        await categoryService.DeleteAsync(id);
        return NoContent();
    }
}

// Модели запросов для категорий
public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class UpdateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}