namespace BussinessLogic.Users.DTOs;

public class CategoryStatsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TopicCount { get; set; }
    public int TodayTopics { get; set; }
}