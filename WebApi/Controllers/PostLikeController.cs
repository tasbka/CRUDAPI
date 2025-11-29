using BussinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

[ApiController]
[Route("api/[controller]")]
public class NoteLikeController(IPostLikeService postLikeService) : ControllerBase
{
    // ЛАЙКНУТЬ заметку
    [HttpPost("{noteId:int}/like")]
    public async Task<IActionResult> LikeNoteAsync([FromRoute] Guid noteId, [FromBody] LikeNoteRequest request)
    {
        await postLikeService.LikeNoteAsync(noteId, request.UserId);
        return Ok();
    }

    // УБРАТЬ лайк с заметки
    [HttpDelete("{noteId:int}/like")]
    public async Task<IActionResult> UnlikeNoteAsync([FromRoute] Guid noteId, [FromBody] UnlikeNoteRequest request)
    {
        await postLikeService.UnlikeNoteAsync(noteId, request.UserId);
        return NoContent();
    }

    // ПОЛУЧИТЬ количество лайков
    [HttpGet("{noteId:int}/likes/count")]
    public async Task<IActionResult> GetLikeCountAsync([FromRoute] Guid noteId)
    {
        var count = await postLikeService.GetLikeCountAsync(noteId);
        return Ok(new { count });
    }

    // ПРОВЕРИТЬ, лайкнул ли пользователь
    [HttpGet("{noteId:int}/likes/check")]
    public async Task<IActionResult> IsNoteLikedAsync([FromRoute] Guid noteId, [FromQuery] Guid userId)
    {
        var isLiked = await postLikeService.IsNoteLikedByUserAsync(noteId, userId);
        return Ok(new { isLiked });
    }
}

// Модели запросов для лайков
public class LikeNoteRequest
{
    public Guid UserId { get; set; }
}

public class UnlikeNoteRequest
{
    public Guid UserId { get; set; }
}