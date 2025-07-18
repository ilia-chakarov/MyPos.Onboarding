using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class RegistrantWithAllWalletsAndUsersDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string DisplayName { get; set; } = null!;
        public string GSM { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsCompany { get; set; }

        public ICollection<UserDto> Users { get; set; } = new List<UserDto>();
        public ICollection<WalletDto> Wallets { get; set; } = new List<WalletDto>();
    }
}
