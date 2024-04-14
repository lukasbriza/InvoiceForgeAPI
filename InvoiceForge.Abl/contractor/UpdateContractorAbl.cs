using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.contractor
{
    public class UpdateContractorAbl: AblBase
    {
        public UpdateContractorAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int contractorId, ContractorUpdateRequest contractor)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(contractor.Owner, "Invalid user Id.");

                    Contractor isContractor = await IsInDatabase<Contractor>(contractorId, "Contractor is not in database.");
                    if (isContractor.Owner != contractor.Owner) throw new ValidationError("Contractor is not in your possession.");

                    if (contractor.AddressId is not null)
                    {
                        Address isAddress = await IsInDatabase<Address>((int)contractor.AddressId, "Address is not in database.");
                        if (isAddress.Owner != isUser.Id) throw new ValidationError("Provided address is not in your possession.");
                    }

                    ClientType? clientType = null;

                    if (contractor.TypeId is not null)
                    {
                        clientType = _repository.CodeLists.GetClientTypeById((int)contractor.TypeId);
                        if (clientType is null) throw new ValidationError("Client type is not in database.");
                    }

                    bool contractorUpdate = await _repository.Contractor.Update(contractorId, contractor, clientType);
                    if (!contractorUpdate) throw new ValidationError("Contractor update failed.");

                    var invoices = await _repository.Invoice.GetByCondition(i => 
                        i.ContractorLocal.Id == contractorId && 
                        i.Owner == isUser.Id && 
                        i.Outdated == false
                    );
                    if (invoices is not null && invoices.Any())
                    {
                        invoices.ConvertAll(i => {
                            i.Outdated = true;
                            return i;
                        });
                    }

                    await SaveResult(contractorUpdate, transaction);
                    return contractorUpdate;
                    
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }
    }
}