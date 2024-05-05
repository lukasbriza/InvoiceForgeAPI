
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.Repository;
using InvoiceForgeApi.Triggers;
using Microsoft.EntityFrameworkCore;

namespace FunctionalTests.Projects.InvoiceForgeApi
{
    public class DatabaseHelper
    {
        public readonly InvoiceForgeDatabaseContext _context;
        public readonly RepositoryWrapper _repository;
        public DatabaseHelper(bool init = true)
        {
            var options = new DbContextOptionsBuilder<InvoiceForgeDatabaseContext>()
                .UseSqlServer("Data Source=LB_NTB;Initial Catalog=InvoiceForgeAPI_Test;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False")
                .UseTriggers(triggerOptions => {
                    triggerOptions.AddTrigger<AddressUpdateTrigger>();
                    triggerOptions.AddTrigger<InvoiceAddressCopyUpdateTrigger>();
                    triggerOptions.AddTrigger<InvoiceEntityCopyUpdateTrigger>();
                    triggerOptions.AddTrigger<ClientUpdateTrigger>();
                    triggerOptions.AddTrigger<ContractorUpdateTrigger>();
                    triggerOptions.AddTrigger<InvoiceTemplateUpdateTrigger>();
                    triggerOptions.AddTrigger<UserAccountUpdateTrigger>();
                    triggerOptions.AddTrigger<InvoiceUserAccountCopyUpdateTrigger>();
                    triggerOptions.AddTrigger<TrackableTrigger>();
                }).Options;
            _context = new InvoiceForgeDatabaseContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();
            
            _repository = new RepositoryWrapper(_context);
            if (init) InitializeDbForTest();
        }
        public void InitializeDbForTest()
        {
            Seed.PopulateDatabase(_context);
            InitTestData();
        }
        public void InitTestData(){
            //User
            if (!_context.User.Any())
            {
                _context.User.AddRange(new UserSeed().Populate());
                _context.SaveChanges();
            }
            //Numbering
            if(!_context.Numbering.Any())
            {
                _context.Numbering.AddRange(new NumberingSeed().Populate());
                _context.SaveChanges();
            }
            //InvoiceItem
            if(!_context.InvoiceItem.Any())
            {
                _context.InvoiceItem.AddRange(new InvoiceItemSeed().Populate());
                _context.SaveChanges();
            }
            //Address
            if (!_context.Address.Any())
            {
                _context.Address.AddRange(new AddressSeed().Populate());
                _context.SaveChanges();
            }
            //Client
            if(!_context.Client.Any()) 
            {
                _context.Client.AddRange(new ClientSeed().Populate());
                _context.SaveChanges();
            }
            //Contractor
            if (!_context.Contractor.Any())
            {
                _context.Contractor.AddRange(new ContractorSeed().Populate());
                _context.SaveChanges();
            }
            //UserAccount
            if (!_context.UserAccount.Any())
            {
                _context.UserAccount.AddRange(new UserAccountSeed().Populate());
                _context.SaveChanges();
            }
            //InvoiceTemplate
            if (!_context.InvoiceTemplate.Any()) 
            {
                _context.InvoiceTemplate.AddRange(new InvoiceTemplateSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitBanks()
        {
            if(!_context.Bank.Any())
            {
                _context.Bank.AddRange(new BankSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitCountries()
        {
            if(!_context.Country.Any())
            {
                _context.Country.AddRange(new CountrySeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitCurrencies()
        {
            if(!_context.Currency.Any())
            {
                _context.Currency.AddRange(new CurrencySeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitTarrifs()
        {
            if(!_context.Tariff.Any())
            {
                _context.Tariff.AddRange(new TariffSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitUsers()
        {
            if (!_context.User.Any())
            {
                _context.User.AddRange(new UserSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitNumberings()
        {
            if(!_context.Numbering.Any())
            {
                _context.Numbering.AddRange(new NumberingSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitInvoiceItems()
        {
            if(!_context.InvoiceItem.Any())
            {
                _context.InvoiceItem.AddRange(new InvoiceItemSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitAddresses()
        {
            if (!_context.Address.Any())
            {
                _context.Address.AddRange(new AddressSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitClients()
        {
            if(!_context.Client.Any()) 
            {
                _context.Client.AddRange(new ClientSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitContractors()
        {
            if (!_context.Contractor.Any())
            {
                _context.Contractor.AddRange(new ContractorSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitUserAccounts()
        {
            if (!_context.UserAccount.Any())
            {
                _context.UserAccount.AddRange(new UserAccountSeed().Populate());
                _context.SaveChanges();
            }
        }
        public void InitInvoiceTemplates()
        {
            if (!_context.InvoiceTemplate.Any()) 
            {
                _context.InvoiceTemplate.AddRange(new InvoiceTemplateSeed().Populate());
                _context.SaveChanges();
            }
        }
        public async Task InitInvoiceCopies()
        {
            if (!_context.InvoiceUserAccountCopy.Any())
            {
                _context.InvoiceUserAccountCopy.AddRange(new InvoiceUserAccountCopySeed().Populate());
                _context.SaveChanges();
            }
            if (!_context.InvoiceAddressCopy.Any())
            {
                _context.InvoiceAddressCopy.AddRange(new InvoiceAddressCopySeed().Populate());
                _context.SaveChanges();
            }
            if (!_context.InvoiceEntityCopy.Any())
            {
                _context.InvoiceEntityCopy.AddRange(new InvoiceEntityCopySeed().Populate());
                _context.SaveChanges();
            }
            if (!_context.Invoice.Any())
            {
                await new InvoiceSeed(_context).Populate();
            }
        }
        public async void  Dispose()
        {
            await _context.Database.EnsureDeletedAsync();
            await  _context.DisposeAsync();
        }
    }
}