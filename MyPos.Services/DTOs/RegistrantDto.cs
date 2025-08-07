using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class RegistrantDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string DisplayName { get; set; } = null!;
        public string GSM { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsCompany { get; set; }
        public int UserId { get; set; }

    }
}
