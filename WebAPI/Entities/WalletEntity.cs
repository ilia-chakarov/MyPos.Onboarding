namespace WebAPI.Entities
{
    public class WalletEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; } = null!;
        public string TarifaCode { get; set; } = null!;
        public string LimitCode { get; set; } = null!;

        public ICollection<UserAccessControlEntity> AccessControls { get; set; } = new List<UserAccessControlEntity>();

        public int RegistrantId { get; set; }
        public RegistrantEntity Registrant { get; set; } = null!;

        public ICollection<AccountEntity> Accounts { get; set; } = new List<AccountEntity>();
    }
}
