using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int RegistrantId { get; set; }
        public RegistrantEntity Registrant { get; set; } = null!;

        public ICollection<UserAccessControlEntity> AccessControls { get; set; } = new List<UserAccessControlEntity>();

    }
}
