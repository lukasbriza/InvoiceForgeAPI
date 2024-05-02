using InvoiceForgeApi.DTO;
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
                    User isUser = await IsInDatabase<User>(client.Owner, "Invalid user Id.");

                    Client isClient = await IsInDatabase<Client>(clientId, "Client is not in database.");
                    if (isClient.Owner != client.Owner) throw new ValidationError("Client is not in your possession.");

                    Address isAddress = await IsInDatabase<Address>(client.AddressId, "Address is not in database.");
                    if (isAddress.Owner != isUser.Id) throw new ValidationError("Provided address is not in your possession.");

                    ClientType? clientType = _repository.CodeLists.GetClientTypeById(client.TypeId);
                    if (clientType is null) throw new ValidationError("Client type is not in database.");

                    bool clientUpdate = await _repository.Client.Update(clientId, client, (ClientType)clientType);
                    if (!clientUpdate) throw new ValidationError("Client update failed.");

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