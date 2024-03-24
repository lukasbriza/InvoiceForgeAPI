
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.Repository;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;

namespace FunctionalTests.Projects.InvoiceForgeAPI
{
    public class DatabaseHelper
    {
        public readonly InvoiceForgeDatabaseContext _context;
        public readonly RepositoryWrapper _repository;
        public DatabaseHelper()
        {
            var options = new DbContextOptionsBuilder<InvoiceForgeDatabaseContext>()
                .UseSqlServer("Data Source=LB_NTB;Initial Catalog=InvoiceForgeAPI_Tests;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False")
                .Options;
            _context = new InvoiceForgeDatabaseContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();
            
            _repository = new RepositoryWrapper(_context);
        }
        public void InitializeDbForTest()
        {
            Seed.PopulateDatabase(_context);
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
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}