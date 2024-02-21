using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface ICodeListsRepository
    {
        Task<List<CountryGetRequest>?> GetCountries();
        Task<List<BankGetRequest>?> GetBanks();
        Task<CodeListsAllGetRequest> GetCodeListsAll();

        List<ClientTypeGetRequest>? GetClientTypes();
    }
}
