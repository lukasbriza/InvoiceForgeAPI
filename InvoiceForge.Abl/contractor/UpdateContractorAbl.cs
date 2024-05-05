using InvoiceForgeApi.Errors;
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
                    User isUser = await IsInDatabase<User>(contractor.Owner);

                    Contractor isContractor = await IsInDatabase<Contractor>(contractorId);
                    if (isContractor.Owner != contractor.Owner) throw new NoPossessionError();

                    Address isAddress = await IsInDatabase<Address>(contractor.AddressId);
                    if (isAddress.Owner != isUser.Id) throw new NoPossessionError();

                    ClientType? clientType = _repository.CodeLists.GetClientTypeById(contractor.TypeId);
                    if (clientType is null) throw new NoEntityError();

                    bool contractorUpdate = await _repository.Contractor.Update(contractorId, contractor, (ClientType)clientType);

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