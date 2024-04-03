using System.Linq.Expressions;
using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;
using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserRepository: IRepositoryBase<UserGetRequest, UserAddRequest, UserUpdateRequest, User> {}
    public interface IUserAccountRepository: IRepositoryBaseExtended<UserAccountGetRequest, UserAccountAddRequest, UserAccountUpdateRequest, UserAccount> {
        public Task<bool> HasDuplicitIbanOrAccountNumber(int userId, UserAccountAddRequest userAccount);
    }
    public interface IInvoiceRepository: IRepositoryBaseExtended<InvoiceGetRequest, InvoiceAddRequestRepository, InvoiceUpdateRequest, Invoice> {}
    public interface IBankRepository: IRepositoryBase<BankGetRequest, BankAddRequest, BankUpdateRequest, Bank> {}
    public interface IAddressRepository: IRepositoryBaseExtended<AddressGetRequest, AddressAddRequest, AddressUpdateRequest, Address> {}
    public interface IInvoiceTemplateRepository: IRepositoryBaseExtended<InvoiceTemplateGetRequest,InvoiceTemplateAddRequest,InvoiceTemplateUpdateRequest, InvoiceTemplate> {}
    public interface IInvoiceItemRepository: IRepositoryBaseExtended<InvoiceItemGetRequest, InvoiceItemAddRequest,InvoiceItemUpdateRequest, InvoiceItem> {}
    public interface IContractorRepository: IRepositoryBaseWithClientExtended<ContractorGetRequest, ContractorAddRequest,ContractorUpdateRequest, Contractor> {}
    public interface IClientRepository: IRepositoryBaseWithClientExtended<ClientGetRequest, ClientAddRequest, ClientUpdateRequest, Client> {}
    public interface IInvoiceServiceRepository: IRepositoryBaseExtended<InvoiceServiceGetRequest, InvoiceServiceExtendedAddRequest,InvoiceServiceUpdateRequest, InvoiceService>{
        public Task<bool> Add(int InvoiceId, List<InvoiceServiceExtendedAddRequest> invoiceServices);
    }
    public interface INumberingRepository
    {
        Task<GenerateInvoiceNumber?> GenerateInvoiceNumber(int numberingId);
        Task<int?> AddNumbering(int userId, NumberingAddRequest numbering);
        Task<bool> UpdateNumbering(int numberingId, NumberingUpdateRequest numbering);
        Task<bool> DeleteNumbering(int numberingId);
    }

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