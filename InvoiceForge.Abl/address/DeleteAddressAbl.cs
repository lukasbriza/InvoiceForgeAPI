using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.address
{
    public class DeleteAddressAbl: AblBase
    {
        public DeleteAddressAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int addressId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    var hasContractReference = await _repository.Contractor.GetByCondition((contractor) => contractor.AddressId == addressId);
                    if (hasContractReference is not null && hasContractReference.Count > 0) 
                        throw new EntityReferenceError();
                    
                    var hasClientReference = await _repository.Client.GetByCondition((client) => client.AddressId == addressId);
                    if (hasClientReference is not null && hasClientReference.Count > 0) 
                        throw new EntityReferenceError();
                    
                    bool deleteAddress = await _repository.Address.Delete(addressId);
                    
                    await SaveResult(deleteAddress, transaction);
                    return deleteAddress;
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