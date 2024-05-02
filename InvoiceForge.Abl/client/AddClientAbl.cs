using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.client
{
    public class AddClientAbl: AblBase
    {
        public AddClientAbl(IRepositoryWrapper repository): base(repository) {}
    
        public async Task<bool> Resolve(int userId, ClientAddRequest client)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(userId, "Invalid user Id.");
                    
                    Address isAddress = await IsInDatabase<Address>(client.AddressId, "Invalid address Id.");
                    if (isAddress.Owner != userId) throw new ValidationError("Provided address is not in your possession.");

                    var isValidClientType = _repository.CodeLists.GetClientTypeById(client.TypeId);
                    if (isValidClientType is null) throw new ValidationError("Provided wrong TypeId.");

                    int? addClient = await _repository.Client.Add(userId, client, (ClientType)isValidClientType);
                    bool saveCondition = addClient is not null;

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