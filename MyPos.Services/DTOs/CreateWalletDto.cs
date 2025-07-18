using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class CreateWalletDto
    {
        public string Status { get; set; } = null!;
        public string TarifaCode { get; set; } = null!;
        public string LimitCode { get; set; } = null!;
        public int RegistrantId { get; set; }
    }
}
