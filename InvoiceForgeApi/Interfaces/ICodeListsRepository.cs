using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface ICodeListsRepository
    {
        public Task<List<CountryGetRequest>> GetCountries();
        public Task<CountryGetRequest?> GetCountryById(int id);
        public Task<List<BankGetRequest>> GetBanks();
        public Task<BankGetRequest?> GetBankById(int id);
        public Task<CodeListsAllGetRequest> GetCodeListsAll();
        public ClientType? GetClientTypeById(int clientTypeId);
        public List<ClientTypeGetRequest> GetClientTypes();
        public List<NumberingVariableGetRequest> GetNumberingVariables();
        public Task<List<TariffGetRequest>> GetTariffs();
        public Task<TariffGetRequest?> GetTariffById(int id);
        public Task<List<CurrencyGetRequest>> GetCurrencies();
        public Task<CurrencyGetRequest?> GetCurrencyById(int id);
    }
}
