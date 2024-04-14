using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
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
                contractors.Include(c => c.Address);
            }
            var contractorsList = await contractors
                .Select(c => new ContractorGetRequest(c, plain))
                .Where(c => c.Owner == userId)
                .ToListAsync();
            return contractorsList;
        }
        public async Task<ContractorGetRequest?> GetById(int contractorId, bool? plain = false)
        {
            DbSet<Contractor> contractor = _dbContext.Contractor;
            if (plain == true)
            {
                contractor.Include(c => c.Address);
            }
            
            var contractorCall = await contractor.FindAsync(contractorId);
            var contractorResult = new ContractorGetRequest(contractorCall, plain);
            return contractorResult;
        }
        public async Task<bool> Update(int contractorId, ContractorUpdateRequest contractor, ClientType? clientType)
        {
            var localContractor = await Get(contractorId);

            if(localContractor is null)
            {
                throw new DatabaseCallError("Contractor is not in database.");
            }

            localContractor.AddressId = contractor.AddressId ?? localContractor.AddressId;
            localContractor.ClientType = clientType ?? localContractor.ClientType;
            localContractor.ContractorName = contractor.ContractorName ?? localContractor.ContractorName;
            localContractor.IN = contractor.IN ?? localContractor.IN;
            localContractor.TIN = contractor.TIN ?? localContractor.TIN;
            localContractor.Email = contractor.Email ?? localContractor.Email;
            localContractor.Mobil = contractor.Mobil ?? localContractor.Mobil;
            localContractor.Tel = contractor.Tel ?? localContractor.Tel;
            localContractor.Www = contractor.Www ?? localContractor.Www;
            
            var update = _dbContext.Update(localContractor);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int userId, ContractorAddRequest contractor)
        {
            var isInDatabase = await _dbContext.Contractor.AnyAsync((c) =>
                c.Owner == userId &&
                c.AddressId == contractor.AddressId &&
                c.ContractorName == contractor.ContractorName &&
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