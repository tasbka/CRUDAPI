using System.Security.Claims;
using BussinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi;


[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    //создание заметки
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AddNoteRequest request)
    {
        try
        {
            await _noteService.CreateAsync(
                request.UserId,
                request.CategoryId,
                request.Title,
                request.Content);

            return Ok(new
            {
                success = true,
                message = "Тема создана успешно"
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
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка сервера"
            });
        }
    }

    // ПОЛУЧЕНИЕ ВСЕХ ЗАМЕТОК
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            Console.WriteLine("Calling NoteService.GetAllNotesAsync...");
            var notes = await _noteService.GetAllNotesAsync();
        
            Console.WriteLine($"Found {notes.Count()} notes");
            
            var formattedNotes = notes.Select(note => new
            {
                note.Id,
                note.Title,
                content = note.Content.Length > 100 
                    ? note.Content.Substring(0, 100) + "..." 
                    : note.Content,
                category = note.CategoryName,
                author = note.AuthorName,
                authorId = note.AuthorId,
                categoryId = note.CategoryId, 
                replies = note.CountComments, 
                views = note.ViewCount,
                likes = note.LikeCount,          
                timestamp = FormatTime(note.Created),
                isPinned = note.IsPinned,
                isSolved = note.IsSolved,
            }).ToList();
        
            return Ok(new 
            {
                success = true,
                data = formattedNotes,
                count = notes.Count()
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled Exception: {ex.Message}");
            return StatusCode(500, new 
            {
                success = false,
                message = "Ошибка при получении тем"
            });
        }
    }
     
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNoteAsync(Guid id)
    {
        Console.WriteLine($"=== GET NOTE BY ID REQUEST: {id} ===");
    
        try
        {
            var note = await _noteService.GetByIdAsync(id);
        
            Console.WriteLine($"Note found: {note.Title}");
        
            return Ok(new 
            {
                success = true,
                data = new
                {
                    note.Id,
                    note.Title,
                    note.Content,
                    category = note.CategoryName,   
                    author = note.AuthorName,
                    authorId = note.AuthorId,
                    categoryId = note.CategoryId,
                    timestamp = FormatTime(note.Created),
                    views = note.ViewCount,
                    likes = note.LikeCount,
                    isPinned = note.IsPinned,
                    isSolved = note.IsSolved
                }
            });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ArgumentException: {ex.Message}");
            return NotFound(new 
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled Exception: {ex.Message}");
            return StatusCode(500, new 
            {
                success = false,
                message = "Ошибка при получении темы"
            });
        }
    }
    
 //получение заметки по ид
  /*  [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNoteAsync([FromRoute] Guid id)
    {
        try
        {
            var result = await _noteService.GetByIdAsync(id);
            return Ok(new
            {
                success = true,
                data = result
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
    }*/

    //Обновление заметки
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateNoteRequest request)
    {
        try
        {
            if (request == null)
            {
                Console.WriteLine("ERROR: Request is null!");
                return BadRequest(new
                {
                    success = false,
                    message = "Request body is null"
                });
            }

            if (string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Content))
            {
                Console.WriteLine("ERROR: No data to update");
                return BadRequest(new
                {
                    success = false,
                    message = "Нет данных для обновления"
                });
            }

            Console.WriteLine("Calling NoteService.UpdateAsync...");
            await _noteService.UpdateAsync(id, request.Title, request.Content);

            Console.WriteLine("Note updated successfully!");

            return Ok(new
            {
                success = true,
                message = "Тема обновлена успешно"
            });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ArgumentException: {ex.Message}");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled Exception: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при обновлении темы"
            });
        }
    }

    // УДАЛЕНИЕ ЗАМЕТКИ
    // DELETE: api/note/{id} 
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNoteAsync([FromRoute] Guid id)
    {
        try
        {
            Console.WriteLine($"Вызов DeleteNoteAsync для id: {id}");
        
            // Вызываем реальный метод удаления из сервиса
            await _noteService.DeleteAsync(id);
        
            Console.WriteLine($"Тема {id} успешно удалена");
        
            return Ok(new
            {
                success = true,
                message = $"Тема успешно удалена",
                id = id,
                timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ArgumentException: {ex.Message}");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled Exception: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при удалении темы",
                error = ex.Message
            });
        }
    }
    
    // PATCH: api/note/{id}/pin - ЗАКРЕПИТЬ/ОТКРЕПИТЬ НАДО УБРАТЬ АВТОРИЗАЦИЮ, ПЕРЕДЕЛАТЬ МЕТОД
   /* [HttpPatch("{id}/pin")]
    public async Task<IActionResult> TogglePinAsync([FromRoute] Guid id, [FromBody] TogglePinRequest request)
    {
        try
        {
            // Получаем информацию о текущем пользователе
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Novice";

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Пользователь не авторизован"
                });
            }

            Console.WriteLine($"Закрепление темы {id} пользователем {currentUserId} с ролью {currentUserRole}");

            await _noteService.TogglePinAsync(
                id, 
                request.IsPinned, 
                Guid.Parse(currentUserId), 
                currentUserRole
            );

            return Ok(new
            {
                success = true,
                message = request.IsPinned ? "Тема закреплена" : "Тема откреплена"
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"UnauthorizedAccessException: {ex.Message}");
            return StatusCode(403, new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ArgumentException: {ex.Message}");
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled Exception: {ex.Message}");
            return StatusCode(500, new
            {
                success = false,
                message = "Ошибка при изменении статуса закрепления",
                details = ex.Message
            });
        }
    }
    */
    // Вспомогательный метод для форматирования времени
    private string FormatTime(DateTime date)
    {
        var timeSpan = DateTime.UtcNow - date;
        
        if (timeSpan.TotalMinutes < 1) return "только что";
        if (timeSpan.TotalHours < 1) return $"{(int)timeSpan.TotalMinutes} мин. назад";
        if (timeSpan.TotalDays < 1) return $"{(int)timeSpan.TotalHours} ч. назад";
        if (timeSpan.TotalDays < 7) return $"{(int)timeSpan.TotalDays} дн. назад";
        
        return date.ToString("dd.MM.yyyy");
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

    public class TogglePinRequest
    {
        public bool IsPinned { get; set; }
    }                                                                                                                                     

    public class UpdateNoteRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
