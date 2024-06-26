using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.address
{
    public class UpdateAddressAbl: AblBase
    {
        public UpdateAddressAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int addressId, AddressUpdateRequest address)
        {
            using (var transaction = await _repository.BeginTransaction()) 
            {
                try
                {
                    await IsInDatabase<User>(address.Owner);
                    await IsInDatabase<Country>(address.CountryId);
                    Address? isAddress = await IsInDatabase<Address>(addressId);
                    if (isAddress?.Owner != address.Owner) throw new NoPossessionError();

                    bool addressUpdate = await _repository.Address.Update(addressId, address);

                    await SaveResult(addressUpdate, transaction);
                    return addressUpdate;
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