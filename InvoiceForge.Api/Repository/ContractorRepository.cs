using InvoiceForgeApi.Data;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class ContractorRepository: 
        RepositoryExtendeClient<Contractor, ContractorAddRequest>, 
        IContractorRepository
    {
        public ContractorRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}

        public async Task<List<ContractorGetRequest>?> GetAll(int userId, bool? plain = false)
        {
            DbSet<Contractor> contractors = _dbContext.Contractor;
            if (plain == false)
            {
                await contractors.Include(c => c.Address).ToListAsync();
            }
            var contractorsList = await contractors
                .Where(c => c.Owner == userId)
                .ToListAsync();
            return contractorsList.ConvertAll(c => new ContractorGetRequest(c, plain));
        }
        public async Task<ContractorGetRequest?> GetById(int contractorId, bool? plain = false)
        {
            DbSet<Contractor> contractor = _dbContext.Contractor;
            if (plain == true)
            {
                await contractor.Include(c => c.Address).LoadAsync();
            }
            
            var contractorCall = await contractor.FindAsync(contractorId);
            var contractorResult = new ContractorGetRequest(contractorCall, plain);
            return contractorResult;
        }
        public async Task<bool> Update(int contractorId, ContractorUpdateRequest contractor, ClientType clientType)
        {
            var localContractor = await Get(contractorId);
            if(localContractor is null) throw new NoEntityError();
            
            var localSelect = new {
                localContractor.AddressId,
                localContractor.Type,
                localContractor.Name,
                localContractor.IN,
                localContractor.TIN,
                localContractor.Email,
                localContractor.Mobil,
                localContractor.Tel,
                localContractor.Www
            };
            var updateSelect = new {
                contractor.AddressId,
                Type = clientType,
                contractor.Name,
                contractor.IN,
                contractor.TIN,
                contractor.Email,
                contractor.Mobil,
                contractor.Tel,
                contractor.Www
            };
            if (localSelect.Equals(updateSelect)) throw new EqualEntityError();

            localContractor.AddressId = contractor.AddressId;
            localContractor.Type = clientType;
            localContractor.Name = contractor.Name;
            localContractor.IN = contractor.IN;
            localContractor.TIN = contractor.TIN;
            localContractor.Email = contractor.Email;
            localContractor.Mobil = contractor.Mobil;
            localContractor.Tel = contractor.Tel;
            localContractor.Www = contractor.Www;
            
            var update = _dbContext.Update(localContractor);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int userId, ContractorAddRequest contractor)
        {
            var isInDatabase = await _dbContext.Contractor.AnyAsync((c) =>
                c.Owner == userId &&
                c.AddressId == contractor.AddressId &&
                c.Name == contractor.Name &&
                c.IN == contractor.IN &&
                c.TIN == contractor.TIN &&
                c.Mobil == contractor.Mobil &&
                c.Tel == contractor.Tel &&
                c.Email == contractor.Email &&
                c.Www == contractor.Www
            );
            return !isInDatabase;
        }
    }
}