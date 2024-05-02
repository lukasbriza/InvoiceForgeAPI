using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
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
                await clients.Include(c => c.Address).ThenInclude(a => a!.Country).LoadAsync();
            }
              
              var clientsList = await clients
                .Where(c => c.Owner == userId)
                .ToListAsync();
                
            return clientsList.ConvertAll(c => new ClientGetRequest(c, plain));


        }
        public async Task<ClientGetRequest?> GetById(int clientId, bool? plain)
        {
            var client = _dbContext.Client;
            if (plain == false)
            {
                await client.Include(c => c.Address).ThenInclude(a => a!.Country).LoadAsync();
            }

            var clientCall = await client.FindAsync(clientId);
            if (clientCall is null) throw new DatabaseCallError("Client is not in database.");
            var clientResult = new ClientGetRequest(clientCall, plain);
            return clientResult;
        }
        
        public async Task<bool> Update(int clientId, ClientUpdateRequest client, ClientType clientType)
        {
            var localClient = await Get(clientId);
            if (localClient is null) throw new DatabaseCallError("Client is not in database.");
            
            var localSelect = new { 
                localClient.AddressId,
                localClient.Type,
                localClient.Name,
                localClient.IN,
                localClient.TIN,
                localClient.Mobil,
                localClient.Tel,
                localClient.Email
            };
            var updateSelect = new {
                client.AddressId,
                Type = clientType,
                client.Name,
                client.IN,
                client.TIN,
                client.Mobil,
                client.Tel,
                client.Email
            };
            if (localSelect.Equals(updateSelect)) throw new ValidationError("One of properties must be different from actual ones.");
            
            localClient.AddressId = client.AddressId;
            localClient.Type = clientType;
            localClient.Name = client.Name;
            localClient.IN = client.IN;
            localClient.TIN = client.TIN;
            localClient.Mobil = client.Mobil;
            localClient.Tel = client.Tel;
            localClient.Email = client.Email;  

            var update = _dbContext.Update(localClient);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int userId, ClientAddRequest client)
        {
            var isInDatabase = await _dbContext.Client.AnyAsync((c) =>
                c.Owner == userId &&
                c.AddressId == client.AddressId &&
                c.Name == client.Name &&
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
