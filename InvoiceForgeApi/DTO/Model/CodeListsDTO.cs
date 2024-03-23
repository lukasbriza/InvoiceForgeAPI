
namespace InvoiceForgeApi.DTO.Model
{
    public class CodeListsAllGetRequest
    {
        public List<CountryGetRequest>? Countries { get; set; } = new List<CountryGetRequest>();
        public List<BankGetRequest>? Banks { get; set; } = new List<BankGetRequest>();
        public List<ClientTypeGetRequest>? ClientTypes { get; set; } = new List<ClientTypeGetRequest>();
        public List<TariffGetRequest>? Tariffs { get; set; } = new List<TariffGetRequest>();
        public List<NumberingVariableGetRequest>? NumberingVariables { get; set; } = new List<NumberingVariableGetRequest>();
        public List<CurrencyGetRequest>? Currencies { get; set; } = new List<CurrencyGetRequest>();
    }
}
