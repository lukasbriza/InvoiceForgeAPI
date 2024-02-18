using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class CodeListsRepository: ICodeListsRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public CodeListsRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CountryGetRequest>?> GetCountries()
        {
            var countryList = await _dbContext.Country.ToListAsync();
            return countryList
                .ConvertAll(c => new CountryGetRequest
                {
                    Id = c.Id,
                    Value = c.Value,
                    Shortcut = c.Shortcut,
                }
            );
        }
        public async Task<List<BankGetRequest>?> GetBanks()
        {
            var bankList = await _dbContext.Bank.ToListAsync();
            return bankList
                .ConvertAll(b => new BankGetRequest
                    {
                        Id = b.Id,
                        Value = b.Value,
                        Shortcut = b.Shortcut,
                        SWIFT = b.SWIFT,
                    }
                );
        }
        public async Task<CodeListsAllGetRequest> GetCodeListsAll()
        {
            var bankList = await GetBanks();
            var countriesList = await GetCountries();

            return new CodeListsAllGetRequest { Banks = bankList, Countries = countriesList };
        }
    }
}
