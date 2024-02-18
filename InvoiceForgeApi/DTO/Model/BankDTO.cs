namespace InvoiceForgeApi.DTO.Model
{
    public class BankGetRequest
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;  
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    }
}
