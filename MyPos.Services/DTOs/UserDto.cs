namespace WebAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public int RegistrantId { get; set; }
    }
}
