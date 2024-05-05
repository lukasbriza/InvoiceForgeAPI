using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.client
{
    public class UpdateClientAbl: AblBase
    {
        public UpdateClientAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int clientId, ClientUpdateRequest client)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(client.Owner);

                    Client isClient = await IsInDatabase<Client>(clientId);
                    if (isClient.Owner != client.Owner) throw new NoPossessionError();

                    Address isAddress = await IsInDatabase<Address>(client.AddressId);
                    if (isAddress.Owner != isUser.Id) throw new NoPossessionError();

                    ClientType? clientType = _repository.CodeLists.GetClientTypeById(client.TypeId);
                    if (clientType is null) throw new NoEntityError();

                    bool clientUpdate = await _repository.Client.Update(clientId, client, (ClientType)clientType);

                    await SaveResult(clientUpdate, transaction);
                    return clientUpdate;
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