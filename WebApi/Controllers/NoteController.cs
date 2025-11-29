using BussinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi;


[ApiController]
[Route("api/[controller]")]
public class NoteController(INoteService noteService) : ControllerBase
{
    //создание заметки
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AddNoteRequest request)
    {
        await noteService.CreateAsync(request.UserId, request.CategoryId, request.Title, request.Content);
        return Ok();
    }
    //получение заметки по ид
    [HttpGet("{id:guid}")]

    public async Task<IActionResult> GetNoteAsync([FromRoute]Guid id)
    {
        var result = await noteService.GetByIdAsync(id);
        return Ok(result);
    }
    
    // ПОЛУЧИТЬ заметки по категории
  /*  [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> GetNotesByCategoryAsync([FromRoute] Guid categoryId)
    {
        var result = await noteService.GetByCategoryAsync(categoryId);
        return Ok(result);
    }*/
    
    // ПОЛУЧИТЬ заметки пользователя
   /* [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetNotesByUserAsync([FromRoute] Guid userId)
    {
        var result = await noteService.GetByUserAsync(userId);
        return Ok(result);
    }*/
    
    [HttpPut( "{id:guid}")]
    public async Task<IActionResult> UpdateNoteAsync([FromRoute] Guid id, [FromBody] UpdateNoteRequest request)
    {
        await noteService.UpdateAsync(id, request.Title, request.Content);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]

    public async Task<IActionResult> DeleteNoteAsync([FromRoute]Guid id)
    {
        await noteService.DeleteAsync(id);
        return NoContent();
    }
}

// Модели запросов для заметок
public class AddNoteRequest
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class UpdateNoteRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}