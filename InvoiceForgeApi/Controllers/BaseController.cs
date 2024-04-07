using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController: ControllerBase
    {
        public readonly IAddressRepository _addressRepository;
        public readonly IUserRepository _userRepository;
        public readonly IUserAccountRepository _userAccountRepository;
        public readonly IContractorRepository _contractorRepository;
        public readonly IClientRepository _clientRepository;
        public readonly ICodeListsRepository _codeListRepository;
        public readonly IInvoiceRepository _invoiceRepository;
        public readonly IInvoiceTemplateRepository _invoiceTemplateRepository;
        public readonly INumberingRepository _numberingRepository;
        public readonly IInvoiceItemRepository _invoiceItemRepository;
        public readonly IInvoiceServiceRepository _invoiceServiceRepository;
        public readonly IRepositoryWrapper _repository;
        protected BaseController(IRepositoryWrapper repository)
        {
            _addressRepository = repository.Address;
            _userRepository = repository.User;
            _contractorRepository = repository.Contractor;
            _clientRepository = repository.Client;
            _codeListRepository = repository.CodeLists;
            _invoiceRepository = repository.Invoice;
            _invoiceTemplateRepository = repository.InvoiceTemplate;
            _numberingRepository = repository.Numbering;
            _invoiceItemRepository = repository.InvoiceItem;
            _userAccountRepository = repository.UserAccount;
            _invoiceServiceRepository = repository.InvoiceService;
            _repository = repository;
        }
        public virtual async Task<TEntity> IsInDatabase<TEntity>(int entityId, string? errorMessage = null) where TEntity: class
        {
            var dbSet = await _repository.GetSet<TEntity>();
            var setName = typeof(TEntity).FullName;
            if (dbSet is null) throw new DatabaseCallError($"There is no {setName} dbSet in given context.");
            var entity = await dbSet.FindAsync(entityId);
            if (entity is null) throw new ValidationError(errorMessage ?? "Entity is not in database.");
           
            return entity;
        }
    }
}