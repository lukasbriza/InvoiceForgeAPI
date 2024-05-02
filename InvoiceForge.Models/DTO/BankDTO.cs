using InvoiceForgeApi.Models.CodeLists;

namespace InvoiceForgeApi.Models.DTO
{
    public class BankBase 
    {
        public string Value { get; set; } = null!;  
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    }
    public class BankGetRequest:BankBase
    {
        public BankGetRequest() {}
        public BankGetRequest(Bank? bank)
        {
            if (bank is not null)
            {
                Id = bank.Id;
                Value = bank.Value;
                Shortcut = bank.Shortcut;
                SWIFT = bank.SWIFT;
            }
        }
        public int Id { get; set; }
    }

    public class BankUpdateRequest: BankAddRequest {};
    public class BankAddRequest:BankBase {}
}
