using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.CodeLists;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl
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
                    bool isCountry = await IsInDatabase<Country>(address.CountryId);
                    bool isUser = await IsInDatabase<User>(userId);
                    bool isAddressUnique = await _repository.Address.IsUnique(userId, address);
                    int? addAddress = await _repository.Address.Add(userId, address);
                    bool saveCondition = addAddress is not null;

                    if (!saveCondition)
                    {
                        throw new DatabaseCallError("Id was not generated.");
                    }

                    await SaveResult(saveCondition, transaction);
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