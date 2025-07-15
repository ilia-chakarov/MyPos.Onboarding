namespace WebAPI.Entities
{
    public class UserAccessControlEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public int WalletId { get; set; }
        public WalletEntity Wallet { get; set; } = null!;

        public string AccessLevel { get; set; } = "Viewer";
    }
}
