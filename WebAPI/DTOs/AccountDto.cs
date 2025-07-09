using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Currency { get; set; } = null!;
        public decimal Balance { get; set; }
        public string IBAN { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public DateTime LastOperationDT { get; set; }
        public decimal BalanceInEuro { get; set; }

        public int WalletId { get; set; }
    }
}
