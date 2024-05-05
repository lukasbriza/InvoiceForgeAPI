using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
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
                    await IsInDatabase<Country>(address.CountryId);
                    await IsInDatabase<User>(userId);

                    bool isAddressUnique = await _repository.Address.IsUnique(userId, address);
                    if (!isAddressUnique) throw new NotUniqueEntityError();
                    
                    int? addAddress = await _repository.Address.Add(userId, address);
                    bool saveCondition = addAddress is not null;

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