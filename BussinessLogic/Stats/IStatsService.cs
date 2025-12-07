using BussinessLogic.Users.DTOs;

namespace BussinessLogic.Stats;

public interface IStatsService
{
    Task<StatsDto> GetForumStatsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ActiveUserDto>> GetActiveUsersAsync(int count = 4, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryStatsDto>> GetCategoryStatsAsync(CancellationToken cancellationToken = default);
}