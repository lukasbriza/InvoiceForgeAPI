using InvoiceForgeApi.Errors;
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
                    User isUser = await IsInDatabase<User>(userId);
                    
                    Address isAddress = await IsInDatabase<Address>(client.AddressId);
                    if (isAddress.Owner != userId) throw new NoPossessionError();

                    var isValidClientType = _repository.CodeLists.GetClientTypeById(client.TypeId);
                    if (isValidClientType is null) throw new NoEntityError();

                    int? addClient = await _repository.Client.Add(userId, client, (ClientType)isValidClientType);
                    bool saveCondition = addClient is not null;

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