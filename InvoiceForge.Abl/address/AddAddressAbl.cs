using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.CodeLists;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.address
{
    public class AddAddressAbl: AblBase
    {
        public AddAddressAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userId, AddressAddRequest address)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    await IsInDatabase<Country>(address.CountryId, "Invalid country id.");
                    await IsInDatabase<User>(userId, "Invalid user Id.");

                    bool isAddressUnique = await _repository.Address.IsUnique(userId, address);
                    if (!isAddressUnique) throw new ValidationError("Address is not unique.");
                    
                    int? addAddress = await _repository.Address.Add(userId, address);
                    bool saveCondition = addAddress is not null;

                    if (!saveCondition)
                    {
                        throw new DatabaseCallError("Id was not generated.");
                    }

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