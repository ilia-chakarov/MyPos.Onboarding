namespace WebAPI.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; } = null!;
        public string TarifaCode { get; set; } = null!;
        public string LimitCode { get; set; } = null!;

        public ICollection<UserAccessControl> AccessControls { get; set; } = new List<UserAccessControl>();

        public int RegistrantId { get; set; }
        public Registrant Registrant { get; set; } = null!;

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
