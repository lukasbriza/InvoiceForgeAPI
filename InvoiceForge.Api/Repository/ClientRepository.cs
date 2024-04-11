using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class ClientRepository: 
        RepositoryExtendeClient<Client, ClientAddRequest>, 
        IClientRepository
    {
        public ClientRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}

        public async Task<List<ClientGetRequest>?> GetAll(int userId, bool? plain)
        {
            DbSet<Client> clients = _dbContext.Client;
            if (plain == true)
            {
                clients.Include(c => c.Address).ThenInclude(a => a!.Country);
            }
              
              var clientsList = await clients
                .Select(c => new ClientGetRequest(c, plain))
                .Where(c => c.Owner == userId)
                .ToListAsync();
                
            return clientsList;


        }
        public async Task<ClientGetRequest?> GetById(int clientId, bool? plain)
        {
            var client = _dbContext.Client;
            if (plain == false)
            {
                client.Include(c => c.Address).ThenInclude(a => a!.Country);
            }

            var clientCall = await client.FindAsync(clientId);
            if (clientCall is null) throw new DatabaseCallError("Client is not in database.");
            var clientResult = new ClientGetRequest(clientCall, plain);
            return clientResult;
        }
        
        public async Task<bool> Update(int clientId, ClientUpdateRequest client, ClientType? clientType)
        {
            var localClient = await Get(clientId);

            if (localClient is null) throw new DatabaseCallError("Client is not in database.");
            
            localClient.AddressId = client.AddressId ?? localClient.AddressId;
            localClient.Type = clientType ?? localClient.Type;
            localClient.ClientName = client.ClientName ?? localClient.ClientName;
            localClient.IN = client.IN ?? localClient.IN;
            localClient.TIN = client.TIN ?? localClient.TIN;
            localClient.Mobil = client.Mobil ?? localClient.Mobil;
            localClient.Tel = client.Tel ?? localClient.Tel;
            localClient.Email = client.Email ?? localClient.Email;  

            var update = _dbContext.Update(localClient);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int userId, ClientAddRequest client)
        {
            var isInDatabase = await _dbContext.Client.AnyAsync((c) =>
                c.Owner == userId &&
                c.AddressId == client.AddressId &&
                c.ClientName == client.ClientName &&
                c.IN == client.IN &&
                c.TIN == client.TIN &&
                c.Mobil == client.Mobil &&
                c.Tel == client.Tel &&
                c.Email == client.Email
            );
            return !isInDatabase;
        }
    }
}
