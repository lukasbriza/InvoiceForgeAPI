namespace InvoiceForgeApi.DTO.Model
{
    public class CodeListsAllGetRequest
    {
        public List<CountryGetRequest>? Countries { get; set; } = new List<CountryGetRequest>();
        public List<BankGetRequest>? Banks { get; set; } = new List<BankGetRequest>();
        public List<ClientTypeGetRequest>? ClientTypes { get; set; } = new List<ClientTypeGetRequest>();
    }
}
