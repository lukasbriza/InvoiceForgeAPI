using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class ContractorRepository: IContractorRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public ContractorRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ContractorGetRequest>?> GetAll(int userId, bool? plain)
        {
            var contractors = _dbContext.Contractor;
            if (plain == false)
            {
                contractors.Include(c => c.Address);
            }
            var contractorsList = await contractors
                .Select(c => new ContractorGetRequest
                    {
                        Id = c.Id,
                        Owner = c.Owner,
                        ClientType = c.ClientType,
                        ContractorName = c.ContractorName,
                        IN = c.IN,
                        TIN = c.TIN,
                        Email = c.Email,
                        Mobil = c.Mobil,
                        Tel = c.Tel,
                        Www = c.Www,
                        Address = plain == false ? new AddressGetRequest
                        {
                            Id = c.Address!.Id,
                            Owner = c.Address.Owner,
                            Street = c.Address.Street,
                            StreetNumber = c.Address.StreetNumber,
                            City = c.Address.City,
                            PostalCode = c.Address.PostalCode,
                            Country = new CountryGetRequest
                            {
                                Id = c.Address.Country!.Id,
                                Value = c.Address.Country.Value,
                                Shortcut = c.Address.Country.Shortcut
                            }
                        } : null
                    }
                ).Where(c => c.Owner == userId).ToListAsync();
            return contractorsList;
        }
        public async Task<ContractorGetRequest?> GetById(int contractorId, bool? plain)
        {
            DbSet<Contractor> contractor = _dbContext.Contractor;
            if (plain == true)
            {
                contractor.Include(c => c.Address);
            }
            
            var contractorList = await contractor
                .Select(c => new ContractorGetRequest
                    {
                        Id = c.Id,
                        Owner = c.Owner,
                        ClientType = c.ClientType,
                        AddressId = (int)c.AddressId!,
                        ContractorName = c.ContractorName,
                        IN = c.IN,
                        TIN = c.TIN,
                        Email = c.Email,
                        Mobil = c.Mobil,
                        Tel = c.Tel,
                        Www = c.Www,
                        Address = plain == false ? new AddressGetRequest
                        {
                            Id = c.Address!.Id,
                            Owner = c.Address.Owner,
                            Street = c.Address.Street,
                            StreetNumber = c.Address.StreetNumber,
                            City = c.Address.City,
                            PostalCode = c.Address.PostalCode,
                            Country = new CountryGetRequest
                            {
                                Id = c.Address.Country!.Id,
                                Value = c.Address.Country.Value,
                                Shortcut = c.Address.Country.Shortcut
                            }
                        } : null
                    }
                ).Where(c => c.Id == contractorId).ToListAsync();
            if (contractorList.Count > 1)
            {
                throw new DatabaseCallError("Something unexpected happened. There are more than one contractor with this ID.");
            }

            return contractorList[0];
        }
        public async Task<bool> Add(int userId, ContractorAddRequest contractor, ClientType clientType)
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
                Tel = contractor.Email,
                Www = contractor.Www
            };
            await _dbContext.Contractor.AddAsync(newContractor);
            return true;
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
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var contractor = await Get(id);

            if(contractor is null)
            {
                throw new DatabaseCallError("Contractor is not in database.");
            }

            _dbContext.Contractor.Remove(contractor);
            return true;
        }
        private async Task<Contractor?> Get(int id)
        {
            return await _dbContext.Contractor.FindAsync(id);
        }
        public async Task<List<Contractor>?> GetByCondition(Expression<Func<Contractor,bool>> condition)
        {
            var result = await _dbContext.Contractor.Where(condition).ToListAsync();
            return result;
        }

    }
}