using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class CreateWalletDto
    {
        public string Status { get; set; } = null!;
        public string TarifaCode { get; set; } = null!;
        public string LimitCode { get; set; } = null!;

        public ICollection<UserAccessControl> AccessControls { get; set; } = new List<UserAccessControl>();

        public int RegistrantId { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
