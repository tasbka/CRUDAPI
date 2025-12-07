namespace BussinessLogic.Users.DTOs;

public class ActiveUserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int PostCount { get; set; }
    public int Reputation { get; set; }
    public DateTime LastActive { get; set; }
    public string Avatar { get; set; } = "👤";
}