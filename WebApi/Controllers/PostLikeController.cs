using BussinessLogic;
using BussinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

[ApiController]
[Route("api/[controller]")]
public class NoteLikeController(IPostLikeService postLikeService) : ControllerBase
{
    // ЛАЙКНУТЬ заметку
    [HttpPost("like")]
    public async Task<IActionResult> LikeNoteAsync([FromBody] ToggleLikeRequestDto request)
    {
        try
        {
            var result = await postLikeService.LikeNoteAsync(request.NoteId, request.UserId);
            return Ok(new { success = true, data = result,  message = "Тема успешно оценена"});
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Ошибка при оценке темы",
                details = ex.Message 
            });
        }
    }

    // УБРАТЬ лайк с заметки
    [HttpDelete("unlike")]
    public async Task<IActionResult> UnlikeNoteAsync([FromBody] ToggleLikeRequestDto request)
    {
        try
        {
            var result = await postLikeService.UnlikeNoteAsync(request.NoteId, request.UserId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Ошибка при удалении оценки",
                details = ex.Message 
            });
        }
    }
    
    // ПЕРЕКЛЮЧИТЬ лайк (like/unlike) - ОСНОВНОЙ МЕТОД
    [HttpPost("toggle")]
    public async Task<IActionResult> ToggleLikeAsync([FromBody] ToggleLikeRequestDto request)
    {
        try
        {
            var result = await postLikeService.ToggleLikeAsync(request.NoteId, request.UserId);
        
            return Ok(new { 
                success = true, 
                data = result, 
                message = result.IsLikedByCurrentUser 
                    ? "Тема оценена" 
                    : "Оценка удалена"
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Ошибка при переключении оценки",
                details = ex.Message 
            });
        }
    }
    
    // ПОЛУЧИТЬ количество лайков
    [HttpGet("{noteId}/count")]
    public async Task<IActionResult> GetLikeCountAsync([FromRoute] Guid noteId)
    { 
    try
    {
        var count = await postLikeService.GetLikeCountAsync(noteId);
        return Ok(new { 
            success = true, 
            data = new { count },
            message = "Количество лайков получено"
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { 
            success = false, 
            message = "Ошибка при получении количества лайков"
        });
    }
    }

    // ПРОВЕРИТЬ, лайкнул ли пользователь
    [HttpGet("{noteId}/check")]
    public async Task<IActionResult> IsNoteLikedAsync([FromRoute] Guid noteId, [FromQuery] Guid userId)
    {
        try
        {
            var isLiked = await postLikeService.IsNoteLikedByUserAsync(noteId, userId);
            return Ok(new { 
                success = true, 
                data = new { isLiked },
                message = "Статус оценки получен"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Ошибка при проверке статуса оценки"
            });
        }
    }
}

