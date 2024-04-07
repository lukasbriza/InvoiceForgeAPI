using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserRepository: 
        IAddSimple<UserAddRequest>, 
        IUpdate<UserUpdateRequest>, 
        IBaseMethods<UserGetRequest, User> 
        {}
    public interface IUserAccountRepository:
        IGetAll<UserAccountGetRequest>, 
        IIsUnique<UserAccountAddRequest>,
        IAdd<UserAccountAddRequest>, 
        IUpdate<UserAccountUpdateRequest>, 
        IBaseMethods<UserAccountGetRequest, UserAccount>
        {
            public Task<bool> HasDuplicitIbanOrAccountNumber(int userId, UserAccountAddRequest userAccount);
        }
    public interface IAddressRepository: 
        IGetAll<AddressGetRequest>, 
        IIsUnique<AddressAddRequest>, 
        IUpdate<AddressUpdateRequest>, 
        IAdd<AddressAddRequest>,
        IBaseMethods<AddressGetRequest, Address>
        {}
    public interface IInvoiceRepository:
        IGetAll<InvoiceGetRequest>,
        IIsUnique<InvoiceAddRequestRepository>, 
        IAdd<InvoiceAddRequestRepository>,
        IUpdate<InvoiceUpdateRequest>, 
        IBaseMethods<InvoiceGetRequest,Invoice>
        {}
    public interface IInvoiceTemplateRepository:
        IGetAll<InvoiceTemplateGetRequest>,
        IIsUnique<InvoiceTemplateAddRequest>, 
        IAdd<InvoiceTemplateAddRequest>,
        IUpdate<InvoiceTemplateUpdateRequest>,
        IBaseMethods<InvoiceTemplateGetRequest, InvoiceTemplate>
        {}
    public interface IInvoiceItemRepository:
        IGetAll<InvoiceItemGetRequest>,
        IIsUnique<InvoiceItemAddRequest>,
        IAdd<InvoiceItemAddRequest>,
        IUpdate<InvoiceItemUpdateRequest>,
        IBaseMethods<InvoiceItemGetRequest, InvoiceItem>
        {}
    public interface IContractorRepository:
        IGetAll<ContractorGetRequest>,
        IIsUnique<ContractorAddRequest>,
        IAddClient<ContractorAddRequest>,
        IUpdateClient<ContractorUpdateRequest>,
        IBaseMethods<ContractorGetRequest,Contractor>
        {}
    public interface IClientRepository:
        IGetAll<ClientGetRequest>,
        IIsUnique<ClientAddRequest>,
        IAddClient<ClientAddRequest>,
        IUpdateClient<ClientUpdateRequest>,
        IBaseMethods<ClientGetRequest, Client>
        {}
    public interface IInvoiceServiceRepository:
        IGetAll<InvoiceServiceGetRequest>,
        IIsUnique<InvoiceServiceExtendedAddRequest>,
        IAdd<InvoiceServiceExtendedAddRequest>,
        IUpdate<InvoiceServiceUpdateRequest>,
        IBaseMethods<InvoiceServiceGetRequest, InvoiceService>
        {
            public Task<bool> Add(int InvoiceId, List<InvoiceServiceExtendedAddRequest> invoiceServices);
        }

    public interface INumberingRepository:
        IDelete,
        IUpdate<NumberingUpdateRequest>,
        IAdd<NumberingAddRequest>
        {
            Task<GenerateInvoiceNumber?> GenerateInvoiceNumber(int numberingId);
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