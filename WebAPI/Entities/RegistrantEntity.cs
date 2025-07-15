namespace WebAPI.Entities
{
    public class RegistrantEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string DisplayName { get; set; } = null!;
        public string GSM { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool isCompany { get; set; }

        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
        public ICollection<WalletEntity> Wallets { get; set; } = new List<WalletEntity>();
    }
}
