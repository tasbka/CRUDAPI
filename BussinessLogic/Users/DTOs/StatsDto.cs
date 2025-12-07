namespace BussinessLogic.Users.DTOs;

public class StatsDto
{
    public int TotalUsers { get; set; }
    public int TotalTopics { get; set; }
    public int TotalReplies { get; set; } // Пока 0
    public int TodayTopics { get; set; }
    public int TodayUsers { get; set; }
}
