namespace WebAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int RegistrantId { get; set; }
        public Registrant Registrant { get; set; } = null!;

        public ICollection<UserAccessControl> AccessControls { get; set; } = new List<UserAccessControl>();

    }
}
