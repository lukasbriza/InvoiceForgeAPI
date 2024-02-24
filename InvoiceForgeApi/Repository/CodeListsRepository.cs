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
        public async Task<CountryGetRequest> GetCountryById(int id)
        {
            var countries = await _dbContext.Country.Select(c => new CountryGetRequest
                {
                    Id = c.Id,
                    Value = c.Value,
                    Shortcut = c.Shortcut,
                }
            ).Where(c => c.Id == id).ToListAsync();

            if (countries is null || countries.Count() != 1) throw new ValidationError("Something unexpected happened. There is more than one country with that id.");
            return countries[0];
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

            return new CodeListsAllGetRequest { Banks = bankList, Countries = countriesList, ClientTypes = clientTypes };
        }
    }
}
