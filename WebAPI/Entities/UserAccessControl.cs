namespace WebAPI.Entities
{
    public class UserAccessControl
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;

        public string AccessLevel { get; set; } = "Viewer";
    }
}
