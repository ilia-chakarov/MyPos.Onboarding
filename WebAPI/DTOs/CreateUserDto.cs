namespace WebAPI.DTOs
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RegistrantId { get; set; }
    }
}
