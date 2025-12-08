using BussinessLogic.Users.DTOs;
using DataAccess;
using DataAccess.Users;
using Microsoft.EntityFrameworkCore;
using AppContext = DataAccess.AppContext;


namespace BussinessLogic.Stats;

public class StatsService : IStatsService
{
    private readonly AppContext _context; 
    
    public StatsService(AppContext context)
    {
        _context = context;
    }
    
    public async Task<StatsDto> GetForumStatsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        
        var totalUsers = await _context.Users
            .CountAsync(u => u.IsActive, cancellationToken);
            
        var totalTopics = await _context.Notes
            .CountAsync(n => n.IsActive, cancellationToken);
            
        var todayTopics = await _context.Notes
            .CountAsync(n => n.IsActive && n.Created.Date == today, cancellationToken);
            
        var todayUsers = await _context.Users
            .CountAsync(u => u.IsActive && u.CreatedAt.Date == today, cancellationToken);
        
        return new StatsDto
        {
            TotalUsers = totalUsers,
            TotalTopics = totalTopics,
            TotalReplies = 0,
            TodayTopics = todayTopics,
            TodayUsers = todayUsers
        };
    }
    
    public async Task<IEnumerable<ActiveUserDto>> GetActiveUsersAsync(int count = 4, CancellationToken cancellationToken = default)
    {
        var activeUsers = await _context.Users
            .Where(u => u.IsActive)
            .OrderByDescending(u => u.Role == "Мудрец" ? 3 : u.Role == "Админ" ? 2 : u.Role == "Эксперт" ? 1 : 0)
            .ThenByDescending(u => u.PostCount)
            .ThenByDescending(u => u.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
        
        return activeUsers.Select(user => new ActiveUserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = GetDisplayRole(user.Role),
            PostCount = user.PostCount,
            Reputation = user.Reputation,
            Avatar = GetAvatarByRole(user.Role)
        });
    }
    
    public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        
        // Получаем все категории
        var categories = await _context.Categories
            .ToListAsync(cancellationToken);
        
        var result = new List<CategoryStatsDto>();
        
        foreach (var category in categories)
        {
            // Считаем темы в категории
            var topicCount = await _context.Notes
                .Where(n => n.IsActive && n.CategoryId == category.Id)
                .CountAsync(cancellationToken);
                
            // Считаем сегодняшние темы в категории
            var todayTopics = await _context.Notes
                .Where(n => n.IsActive && n.CategoryId == category.Id && n.Created.Date == today)
                .CountAsync(cancellationToken);
            
            result.Add(new CategoryStatsDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description ?? string.Empty,
                TopicCount = topicCount,
                TodayTopics = todayTopics
            });
        }
        
        return result.OrderByDescending(c => c.TopicCount);
    }
    
    private string GetDisplayRole(string role)
    {
        return role switch
        {
            "Novice" => "Новичок",
            "Expert" => "Эксперт",
            "Мудрец" => "Мудрец",
            "Admin" or "Админ" => "Админ",
            _ => "Пользователь"
        };
    }
    
    private string GetAvatarByRole(string role)
    {
        return role switch
        {
            "Admin" or "Админ" => "👑",
            "Мудрец" => "🧙‍♂️",
            "Expert" => "🎓",
            _ => "👤"
        };
    }
}