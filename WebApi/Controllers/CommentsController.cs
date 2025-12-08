using BussinessLogic.Comments;
using BussinessLogic.Comments.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using CreateCommentRequest = BussinessLogic.Comments.DTOs.CreateCommentRequest;
using UpdateCommentRequest = BussinessLogic.Comments.DTOs.UpdateCommentRequest;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    
    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    /// <summary>
    /// Получить все комментарии для темы
    /// </summary>
    [HttpGet("note/{noteId:guid}")]
    public async Task<IActionResult> GetCommentsByNoteId(Guid noteId)
    {
        try
        {
            var comments = await _commentService.GetCommentsByNoteIdAsync(noteId);
            
            return Ok(new
            {
                success = true,
                data = comments,
                count = comments.Count()
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting comments: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при получении комментариев"
            });
        }
    }
    
    /// <summary>
    /// Получить комментарии пользователя
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetCommentsByUserId(Guid userId)
    {
        try
        {
            var comments = await _commentService.GetCommentsByUserIdAsync(userId);
            
            return Ok(new
            {
                success = true,
                data = comments,
                count = comments.Count()
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user comments: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при получении комментариев пользователя"
            });
        }
    }
    
    /// <summary>
    /// Получить конкретный комментарий по ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCommentById(Guid id)
    {
        try
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            
            return Ok(new
            {
                success = true,
                data = comment
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting comment: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при получении комментария"
            });
        }
    }
    
    /// <summary>
    /// Создать новый комментарий
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new 
                { 
                    success = false, 
                    message = "Тело запроса не может быть пустым" 
                });
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                { 
                    success = false, 
                    message = "Некорректные данные",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }
            
            // Маппинг из запроса API в DTO бизнес-логики
            var createDto = new CreateCommentDto
            {
                NoteId = request.NoteId,
                AuthorId = request.AuthorId,
                Content = request.Content,
                ParentCommentId = request.ParentCommentId
            };
            
            var comment = await _commentService.CreateCommentAsync(createDto);
            
            return Ok(new
            {
                success = true,
                message = "Комментарий успешно добавлен",
                data = comment
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating comment: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при создании комментария"
            });
        }
    }
    
    /// <summary>
    /// Обновить комментарий
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateComment(Guid id, [FromBody] UpdateCommentRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new 
                { 
                    success = false, 
                    message = "Тело запроса не может быть пустым" 
                });
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                { 
                    success = false, 
                    message = "Некорректные данные",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }
            
            // Маппинг из запроса API в DTO бизнес-логики
            var updateDto = new UpdateCommentDto
            {
                Content = request.Content
            };
            
            var comment = await _commentService.UpdateCommentAsync(id, updateDto);
            
            return Ok(new
            {
                success = true,
                message = "Комментарий успешно обновлен",
                data = comment
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating comment: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при обновлении комментария"
            });
        }
    }
    
    /// <summary>
    /// Удалить комментарий
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        try
        {
            await _commentService.DeleteCommentAsync(id);
            
            return Ok(new
            {
                success = true,
                message = "Комментарий успешно удален"
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting comment: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при удалении комментария"
            });
        }
    }
    
    /// <summary>
    /// Получить количество комментариев для темы
    /// </summary>
    [HttpGet("note/{noteId:guid}/count")]
    public async Task<IActionResult> GetCommentCount(Guid noteId)
    {
        try
        {
            var count = await _commentService.GetCommentCountByNoteIdAsync(noteId);
            
            return Ok(new
            {
                success = true,
                data = new { count }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting comment count: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при получении количества комментариев"
            });
        }
    }
    
    /// <summary>
    /// Получить количество комментариев пользователя
    /// </summary>
    [HttpGet("user/{userId:guid}/count")]
    public async Task<IActionResult> GetUserCommentCount(Guid userId)
    {
        try
        {
            var count = await _commentService.GetCommentCountByUserIdAsync(userId);
            
            return Ok(new
            {
                success = true,
                data = new { count }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user comment count: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при получении количества комментариев пользователя"
            });
        }
    }
    
    /// <summary>
    /// Получить ответы на комментарий
    /// </summary>
    [HttpGet("{id:guid}/replies")]
    public async Task<IActionResult> GetReplies(Guid id)
    {
        try
        {
            var replies = await _commentService.GetRepliesAsync(id);
            
            return Ok(new
            {
                success = true,
                data = replies,
                count = replies.Count()
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting replies: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при получении ответов"
            });
        }
    }
}