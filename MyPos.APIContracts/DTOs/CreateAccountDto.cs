using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class CreateAccountDto
    {
        public string Currency { get; set; } = null!;
        public decimal Balance { get; set; }
        public string IBAN { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public decimal BalanceInEuro { get; set; }

        public int WalletId { get; set; }
    }
}
