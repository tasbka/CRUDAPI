using BussinessLogic.Stats;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;
        
        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }
        
        [HttpGet("forum")]
        public async Task<IActionResult> GetForumStats()
        {
            try
            {
                var stats = await _statsService.GetForumStatsAsync();
                return Ok(new
                {
                    success = true,
                    data = stats
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ошибка при получении статистики"
                });
            }
        }
        
        [HttpGet("active-users")]
        public async Task<IActionResult> GetActiveUsers([FromQuery] int count = 4)
        {
            try
            {
                var users = await _statsService.GetActiveUsersAsync(count);
                return Ok(new
                {
                    success = true,
                    data = users
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ошибка при получении активных пользователей"
                });
            }
        }
        
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategoryStats()
        {
            try
            {
                var categories = await _statsService.GetCategoryStatsAsync();
                return Ok(new
                {
                    success = true,
                    data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ошибка при получении статистики категорий"
                });
            }
        }
    }