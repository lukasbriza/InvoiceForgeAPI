using InvoiceForgeApi.Data;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InvoiceForgeApi.Repository
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private readonly InvoiceForgeDatabaseContext _context = null!;
        private IUserRepository _user = null!;
        private IUserAccountRepository _userAccount = null!;
        private IClientRepository _client = null!;
        private IAddressRepository _address = null!;
        private ICodeListsRepository _codeLists = null!;
        private IContractorRepository _contractor = null!;
        private IInvoiceTemplateRepository _invoiceTemplate = null!;
        private IInvoiceItemRepository _invoiceItem = null!;
        private IInvoiceServiceRepository _invoiceService = null!;
        private IInvoiceRepository _invoice = null!;
        private IInvoiceEntityCopyRepository _invoiceEntityCopy = null!;
        private IInvoiceAddressCopyRepository _invoiceAddressCopy = null!;
        private IInvoiceUserAccountCopyRepository _invoiceUserAccountCopy = null!;
        private INumberingRepository _numbering = null!;

        public RepositoryWrapper(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }
        public IUserRepository User {
            get {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }
        public IUserAccountRepository UserAccount
        {
            get {
                if (_userAccount == null)
                {
                    _userAccount = new UserAccountRepository(_context);
                }
                return _userAccount;
            }
        }
        public IClientRepository Client
        {
            get {
                if (_client == null)
                {
                    _client = new ClientRepository(_context);
                }
                return _client;
            }
        }
        public IAddressRepository Address
        {
            get {
                if (_address == null)
                {
                    _address = new AddressRepository(_context);
                }
                return _address;
            }
        }
        public IContractorRepository Contractor
        {
            get{
                if (_contractor == null)
                {
                    _contractor = new ContractorRepository(_context);
                }
                return _contractor;
            }
        }
        public IInvoiceTemplateRepository InvoiceTemplate
        {
            get{
                if (_invoiceTemplate == null)
                {
                    _invoiceTemplate = new InvoiceTemplateRepository(_context);
                }
                return _invoiceTemplate;
            }
        }
        public IInvoiceItemRepository InvoiceItem
        {
            get{
                if (_invoiceItem == null)
                {
                    _invoiceItem = new InvoiceItemRepository(_context);
                }
                return _invoiceItem;
            }
        }
        public IInvoiceServiceRepository InvoiceService
        {
            get{
                if (_invoiceService == null)
                {
                    _invoiceService = new InvoiceServiceRepository(_context);
                }
                return _invoiceService;
            }
        }
        public INumberingRepository Numbering
        {
            get{
                if (_numbering == null)
                {
                    _numbering = new NumberingRepository(_context);
                }
                return _numbering;
            }
        }
        public IInvoiceRepository Invoice
        {
            get{
                if (_invoice == null)
                {
                    _invoice = new InvoiceRepository(_context);
                }
                return _invoice;
            }
        }
        public IInvoiceEntityCopyRepository InvoiceEntityCopy
        {
            get{
                if (_invoiceEntityCopy == null)
                {
                    _invoiceEntityCopy = new InvoiceEntityCopyRepository(_context);
                }
                return _invoiceEntityCopy;
            }
        }
        public IInvoiceAddressCopyRepository InvoiceAddressCopy
        {
            get{
                if (_invoiceAddressCopy == null)
                {
                    _invoiceAddressCopy = new InvoiceAddressCopyRepository(_context);
                }
                return _invoiceAddressCopy;
            }
        }
        public IInvoiceUserAccountCopyRepository InvoiceUserAccountCopy
        {
            get{
                if (_invoiceUserAccountCopy == null)
                {
                    _invoiceUserAccountCopy = new InvoiceUserAccountCopyRepository(_context);
                }
                return _invoiceUserAccountCopy;
            }
        }
        public ICodeListsRepository CodeLists
        {
            get {
                if (_codeLists == null)
                {
                    _codeLists = new CodeListsRepository(_context);
                }
                return _codeLists;
            }
        }
        public async Task Save()
        {
            int save = await _context.SaveChangesAsync();
            if (!(save > 0))
            {
                throw new ContextSaveError();
            }
        }
        public async Task<DbSet<TEntity>?> GetSet<TEntity>() where TEntity: class
        {
            var set =  _context.Set<TEntity>();
            return await Task.FromResult(set);
        }
        public void DetachChanges()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Unchanged);
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return transaction;
        }
    }
}