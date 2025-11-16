using BussinessLogic;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi;


[ApiController]
[Route("Note")]
public class NoteController(INoteService noteService) : ControllerBase
{
    //создание заметки
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AddNoteRequest noteRequest)
    {
        await noteService.CreateAsync(noteRequest.Text);
        return Ok();
    }
    //получение заметки по ид
    [HttpGet( "{id:guid}")]

    public async Task<IActionResult> GetNoteAsync([FromRoute]Guid id)
    {
        var result = await noteService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPut( "{id:guid}")]

    public async Task<IActionResult> UpdateNoteAsync([FromRoute]Guid id, CancellationToken newText)
    {
        await noteService.GetByIdAsync(id, newText);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]

    public async Task<IActionResult> DeleteNoteAsync([FromRoute]Guid id)
    {
        await noteService.DeleteAsync(id);
        return NoContent();
    }
}