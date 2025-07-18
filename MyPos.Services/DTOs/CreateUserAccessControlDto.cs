using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class CreateUserAccessControlDto
    {
        public int UserId { get; set; }

        public int WalletId { get; set; }

        public string AccessLevel { get; set; } = "Viewer";
    }
}
