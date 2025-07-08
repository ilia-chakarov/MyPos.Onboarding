namespace WebAPI.DTOs
{
    public class WalletDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; } = null!;
        public string TarifaCode { get; set; } = null!;
        public string LimitCode { get; set; } = null!;
        public int RegistrantId { get; set; }
    }
}
