using InvoiceForgeApi.Data;
using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class ContractorRepository: RepositoryBase<Contractor>, IContractorRepository
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
        public async Task<int?> Add(int userId, ContractorAddRequest contractor, ClientType clientType)
        {
            var newContractor = new Contractor
            {
                AddressId = contractor.AddressId,
                Owner = userId,
                ClientType = clientType,
                ContractorName = contractor.ContractorName,
                IN = contractor.IN,
                TIN = contractor.TIN,
                Email = contractor.Email,
                Mobil = contractor.Mobil,
                Tel = contractor.Tel,
                Www = contractor.Www
            };
            var entity = await _dbContext.Contractor.AddAsync(newContractor);
            
            if (entity.State == EntityState.Added) await _dbContext.SaveChangesAsync();
            return entity.State == EntityState.Unchanged ? entity.Entity.Id : null;
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
    }
}