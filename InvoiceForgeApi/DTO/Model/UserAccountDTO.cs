namespace InvoiceForgeApi.DTO.Model
{
    public class UserAccountGetRequest
    {
        public int Id { get; set; }
        public int Owner { get; set; }
        public int? BankId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; }

        public BankGetRequest Bank { get; set; } = null!;
    }
}
