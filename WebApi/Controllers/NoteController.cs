using BussinessLogic;
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
        await noteService.CreateAsync(noteRequest.UserId, noteRequest.Text);
        return Ok();
    }
    //получение заметки по ид
    [HttpGet( "{id:int}")]

    public async Task<IActionResult> GetNoteAsync([FromRoute]int id)
    {
        var result = await noteService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPut( "{id:int}")]

    public async Task<IActionResult> UpdateNoteAsync([FromRoute]int id, CancellationToken newText)
    {
        await noteService.GetByIdAsync(id, newText);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]

    public async Task<IActionResult> DeleteNoteAsync([FromRoute]int id)
    {
        await noteService.DeleteAsync(id);
        return NoContent();
    }
}