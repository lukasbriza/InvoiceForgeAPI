using InvoiceForgeApi.DTO;
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
                    await IsInDatabase<User>(address.Owner, "Invalid user Id.");
                    await IsInDatabase<Country>(address.CountryId, "Provided countryId is invalid.");
                    Address? isAddress = await IsInDatabase<Address>(addressId, "Invalid Address Id.");
                    if (isAddress?.Owner != address.Owner) throw new ValidationError("Provided address is not in your possession.");

                    bool addressUpdate = await _repository.Address.Update(addressId, address);
                    if (!addressUpdate) throw new ValidationError("Address update failed.");

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