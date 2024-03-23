using InvoiceForgeApi.Data;
using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
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
            var country = await _dbContext.Country
                .Select(c => new CountryGetRequest(c))
                .Where(c => c.Id == id)
                .ToListAsync();

            if (country.Count > 1) throw new ValidationError("Something unexpected happened. There is more than one country with that id.");
            if (country is null || country.Count == 0) return null;
            return country[0];
        }
        public async Task<List<BankGetRequest>> GetBanks()
        {
            var bankList = await _dbContext.Bank.ToListAsync();
            return bankList
                .ConvertAll(b => new BankGetRequest(b));
        }
        public async Task<BankGetRequest?> GetBankById(int id)
        {
            var bank = await _dbContext.Bank
                .Select(b => new BankGetRequest(b))
                .Where(b => b.Id == id)
                .ToListAsync();

            if (bank is null || bank.Count == 0) return null;
            if (bank.Count > 1) throw new ValidationError("Something unexpected happened. There is more than one bank with that id.");
            return bank[0];
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
            var tariff = await _dbContext.Tariff
                .Select(t => new TariffGetRequest(t))
                .Where(t => t.Id == id)
                .ToListAsync();
            if (tariff is null || tariff.Count == 0) return null;
            if (tariff.Count > 1) throw new ValidationError("Something unexpected happened. There is more than one tariff with that id.");
            return tariff[0];
        }
        public async Task<List<CurrencyGetRequest>> GetCurrencies()
        {
            var currencies = await _dbContext.Currency.ToListAsync();
            return currencies.ConvertAll(c => new CurrencyGetRequest(c));
        }
        public async Task<CurrencyGetRequest?> GetCurrencyById(int id)
        {
            var currency = await _dbContext.Currency
                .Select(c => new CurrencyGetRequest(c))
                .Where(c => c.Id == id)
                .ToListAsync();
            if (currency is null || currency.Count == 0) return null;
            if (currency.Count > 1) throw new ValidationError("Something unexpected happened. There is more than one currency with that id.");
            return currency[0];
        }
    }
}
