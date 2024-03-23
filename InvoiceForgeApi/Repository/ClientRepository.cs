using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class ClientRepository: IClientRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public ClientRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
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
            DbSet<Client> client = _dbContext.Client;
            if (plain == false)
            {
                client.Include(c => c.Address).ThenInclude(a => a!.Country);
            }

            var clientList = await client
                .Select(c => new ClientGetRequest(c, plain))
                .Where(c => c.Id == clientId)
                .ToListAsync();

            if (clientList.Count > 1)
            {
                throw new DatabaseCallError("Something unexpected happened. There are more than one client with this ID.");
            }

            return clientList[0];
        }
        public async Task<int?> Add(int userId ,ClientAddRequest client, ClientType clientType)
        {
            var newClient = new Client
            {
                AddressId = client.AddressId,
                Owner = userId,
                Type = clientType,
                ClientName = client.ClientName,
                IN = client.IN,
                TIN = client.TIN,
                Mobil = client.Mobil,
                Tel = client.Tel,
                Email = client.Email
            };
            var entity = await _dbContext.Client.AddAsync(newClient);
            return entity.State == EntityState.Added ? entity.Entity.Id : null;
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

            return _dbContext.Entry(localClient).State == EntityState.Modified;
        }
        public async Task<bool> Delete(int id)
        {
            var client = await Get(id);
            if (client is null) throw new DatabaseCallError("Client is not in database.");    

            var entity = _dbContext.Client.Remove(client);
            return entity.State == EntityState.Deleted;
        }
        private async Task<Client?> Get(int id)
        {
            return await _dbContext.Client.FindAsync(id);
        }
        public async Task<List<Client>?> GetByCondition(Expression<Func<Client,bool>> condition)
        {
            var result = await _dbContext.Client.Where(condition).ToListAsync();
            return result;
        }
    }
}
