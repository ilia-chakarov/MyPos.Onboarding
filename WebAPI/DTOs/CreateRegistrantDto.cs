namespace WebAPI.DTOs
{
    public class CreateRegistrantDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string GSM { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsCompany { get; set; }
    }
}
