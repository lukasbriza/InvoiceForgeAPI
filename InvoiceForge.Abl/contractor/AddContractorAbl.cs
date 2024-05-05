using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.contractor
{
    public class AddContractorAbl: AblBase
    {
        public AddContractorAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userId, ContractorAddRequest contractor)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    await IsInDatabase<User>(userId);
                    
                    var isAddress = await IsInDatabase<Address>(contractor.AddressId);
                    if (isAddress.Owner != userId) throw new NoPossessionError();

                    var isClientType = _repository.CodeLists.GetClientTypeById(contractor.TypeId);
                    if (isClientType is null) throw new NoEntityError();

                    int? addContractor = await _repository.Contractor.Add(userId, contractor, (ClientType)isClientType);
                    bool saveCondition = addContractor is not null;

                    await SaveResult(saveCondition, transaction, false);
                    return saveCondition;
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