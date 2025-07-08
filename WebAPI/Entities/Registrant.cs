namespace WebAPI.Entities
{
    public class Registrant
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string DisplayName { get; set; } = null!;
        public string GSM { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool isCompany { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
