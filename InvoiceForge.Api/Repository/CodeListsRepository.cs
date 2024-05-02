using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace InvoiceForgeApi.Repository
{
    public class CodeListsRepository: ICodeListsRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public CodeListsRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CountryGetRequest>> GetCountries()
        {
            var countryList = await _dbContext.Country.ToListAsync();
            return countryList.ConvertAll(c => new CountryGetRequest(c));
        }
        public async Task<CountryGetRequest?> GetCountryById(int id)
        {
            var country = await _dbContext.Country.FindAsync(id);
            var countryResult = new CountryGetRequest(country);
            return country is not null ? countryResult : null;
        }
        public async Task<List<BankGetRequest>> GetBanks()
        {
            var bankList = await _dbContext.Bank.ToListAsync();
            return bankList.ConvertAll(b => new BankGetRequest(b));
        }
        public async Task<BankGetRequest?> GetBankById(int id)
        {
            var bank = await _dbContext.Bank.FindAsync(id);
            var bankresult = new BankGetRequest(bank);
            return bank is not null ? bankresult : null;
        }
        public List<ClientTypeGetRequest> GetClientTypes()
        {
            var list = new List<ClientTypeGetRequest>()
            {
                new ClientTypeGetRequest()
                {
                    Id = (int)ClientType.LegalEntity,
                    Description = ClientType.LegalEntity.GetDisplayName()
                },
                new ClientTypeGetRequest()
                {
                    Id = (int)ClientType.Entrepreneur,
                    Description = ClientType.Entrepreneur.GetDisplayName()
                }
            };
            return list;
        }
        public ClientType? GetClientTypeById(int clientTypeId)
        {
            var list = GetClientTypes();
            var clientType = list?.Find(c=>c.Id == clientTypeId);

            
            if(clientType is null)
            {
                return null;
            }
            
            var id = clientType.Id;
            return (ClientType)id;
        }
        public async Task<CodeListsAllGetRequest> GetCodeListsAll()
        {
            var bankList = await GetBanks();
            var countriesList = await GetCountries();
            var clientTypes =  GetClientTypes();
            var numberingVariables = GetNumberingVariables();
            var currencies = await GetCurrencies();
            var tariffs = await GetTariffs();

            return new CodeListsAllGetRequest { 
                Banks = bankList, 
                Countries = countriesList, 
                ClientTypes = clientTypes, 
                NumberingVariables = numberingVariables,
                Currencies = currencies,
                Tariffs = tariffs
            };
        }
        public List<NumberingVariableGetRequest> GetNumberingVariables()
        {
            var list = new List<NumberingVariableGetRequest>()
            {
                new NumberingVariableGetRequest()
                {
                    Id = (int)NumberingVariable.Number,
                    Value = NumberingVariable.Number.GetDisplayName()
                },
                new NumberingVariableGetRequest()
                {
                    Id = (int)NumberingVariable.Day,
                    Value = NumberingVariable.Day.GetDisplayName()
                },
                new NumberingVariableGetRequest()
                {
                    Id = (int)NumberingVariable.Month,
                    Value = NumberingVariable.Month.GetDisplayName()
                },
                new NumberingVariableGetRequest()
                {
                    Id = (int)NumberingVariable.Year,
                    Value = NumberingVariable.Year.GetDisplayName()
                }
            };
            return list;
        }
        public async Task<List<TariffGetRequest>> GetTariffs()
        {
            var tariffs = await _dbContext.Tariff.ToListAsync();
            return tariffs.ConvertAll(t => new TariffGetRequest
                {
                    Id = t.Id,
                    Value = t.Value
                }
            );
        }
        public async Task<TariffGetRequest?> GetTariffById(int id)
        {
            var tariff = await _dbContext.Tariff.FindAsync(id);
            var tariffResult = new TariffGetRequest(tariff);
            return tariff is not null ? tariffResult : null;
        }
        public async Task<List<CurrencyGetRequest>> GetCurrencies()
        {
            var currencies = await _dbContext.Currency.ToListAsync();
            return currencies.ConvertAll(c => new CurrencyGetRequest(c));
        }
        public async Task<CurrencyGetRequest?> GetCurrencyById(int id)
        {
            var currency = await _dbContext.Currency.FindAsync(id);
            var currencyResult = new CurrencyGetRequest(currency);
            return currency is not null ? currencyResult : null;
        }
    }
}
