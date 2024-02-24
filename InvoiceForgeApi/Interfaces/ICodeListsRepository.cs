using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface ICodeListsRepository
    {
        Task<List<CountryGetRequest>?> GetCountries();
        Task<CountryGetRequest> GetCountryById(int id);
        Task<List<BankGetRequest>?> GetBanks();
        Task<CodeListsAllGetRequest> GetCodeListsAll();
        public ClientType? GetClientTypeById(int clientTypeId);
        List<ClientTypeGetRequest>? GetClientTypes();
    }
}
