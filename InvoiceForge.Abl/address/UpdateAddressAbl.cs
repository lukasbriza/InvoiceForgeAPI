using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.CodeLists;
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
                    User? user = await IsInDatabase<User>(address.Owner, "Invalid user Id.");
                    Address? isAddress = await IsInDatabase<Address>(addressId, "Invalid Address Id.");
                    if (isAddress?.Owner != address.Owner) throw new ValidationError("Provided address is not in your possession.");

                    var countryId = address.CountryId;  
                    if (countryId is not null) await IsInDatabase<Country>((int)countryId, "Provided countryId is invalid.");
                    

                    var isAddressUnique = await _repository.Address.IsUnique(user.Id, address);
                    if (!isAddressUnique) throw new ValidationError("Address must be unique.");

                    bool addressUpdate = await _repository.Address.Update(addressId, address);
                    if (!addressUpdate) throw new ValidationError("Address update failed.");
                  
                    var invoices = await _repository.Invoice.GetByCondition(i => 
                        (i.ClientLocal.AddressId == addressId || i.ContractorLocal.AddressId == addressId) && 
                        i.Outdated == false
                    );
                    if (invoices is not null && invoices.Any())
                    {
                        invoices.ConvertAll(c => {
                            c.Outdated = true;
                            return c;
                        });
                    }

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