using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Interfaces;

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
                if(_address == null)
                {
                    _address = new AddressRepository(_context);
                }
                return _address;
            }
        }
        public IContractorRepository Contractor
        {
            get{
                if(_contractor == null)
                {
                    _contractor = new ContractorRepository(_context);
                }
                return _contractor;
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
            if(!(save > 0))
            {
                throw new DatabaseCallError("Saving failed. Modifications was not applied.");
            }
        }
    }
}