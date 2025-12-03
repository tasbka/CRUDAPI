namespace BussinessLogic.Users.DTOs
{

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public int Reputation { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}