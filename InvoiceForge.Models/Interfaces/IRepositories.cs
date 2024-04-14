using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Models.Interfaces
{
    public interface IAddressRepository: IBaseRepository<AddressGetRequest, AddressAddRequest, AddressUpdateRequest, Address> {}
    public interface IClientRepository: IBaseClientRepository<ClientGetRequest, ClientAddRequest, ClientUpdateRequest, Client> {}
    public interface IContractorRepository: IBaseClientRepository<ContractorGetRequest, ContractorAddRequest, ContractorUpdateRequest, Contractor> {}
    public interface IInvoiceItemRepository: IBaseRepository<InvoiceItemGetRequest, InvoiceItemAddRequest, InvoiceItemUpdateRequest, InvoiceItem> {}
    public interface IInvoiceRepository: IBaseRepository<InvoiceGetRequest, InvoiceAddRequestRepository, InvoiceUpdateRequest, Invoice> {}
    public interface IInvoiceServiceRepository: IBaseRepository<InvoiceServiceGetRequest, InvoiceServiceExtendedAddRequest, InvoiceServiceUpdateRequest, InvoiceService>, IAddRange<InvoiceServiceExtendedAddRequest> {}
    public interface IInvoiceTemplateRepository: IBaseRepository<InvoiceTemplateGetRequest, InvoiceTemplateAddRequest, InvoiceTemplateUpdateRequest, InvoiceTemplate> {}
    public interface IUserAccountRepository: IBaseRepository<UserAccountGetRequest, UserAccountAddRequest, UserAccountUpdateRequest, UserAccount>
    {
        public Task<bool> HasDuplicitIbanOrAccountNumber(int userId, UserAccountAddRequest userAccount);
    }
    public interface IUserRepository:
        IAddSimple<UserAddRequest>, 
        IUpdate<UserUpdateRequest>, 
        IBaseMethods<UserGetRequest, User>
        {}

    public interface INumberingRepository:
        IDelete,
        IUpdate<NumberingUpdateRequest>,
        IAdd<NumberingAddRequest>
    {
        Task<GenerateInvoiceNumber?> GenerateInvoiceNumber(int numberingId);
    }
    public interface IBaseRepository<TGetRequest, TAddRequest, TUpdateRequest, TEntity>:
        IGetAll<TGetRequest>, 
        IIsUnique<TAddRequest>, 
        IAdd<TAddRequest>,
        IUpdate<TUpdateRequest>, 
        IBaseMethods<TGetRequest, TEntity>
        {}
    
    public interface IBaseClientRepository<TGetRequest, TAddRequest, TUpdateRequest, TEntity>:
        IGetAll<TGetRequest>, 
        IIsUnique<TAddRequest>, 
        IAddClient<TAddRequest>,
        IUpdateClient<TUpdateRequest>, 
        IBaseMethods<TGetRequest, TEntity>
        {}

    public interface IAddRange<TAddRequest> {
        public Task<bool> Add(int InvoiceId, List<TAddRequest> invoiceServices);
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