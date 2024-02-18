namespace InvoiceForgeApi.DTO.Model
{
    public class BankBase 
    {
        public string Value { get; set; } = null!;  
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    }
    public class BankGetRequest
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;  
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    }

    public class BankUpdateRequest
    {
        public string? Value { get; set; } = null!;  
        public string? Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    };

    public class BankAddRequest 
    {
        public string Value { get; set; } = null!;  
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }   
    }
}
