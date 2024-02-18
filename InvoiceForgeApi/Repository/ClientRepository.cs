using InvoiceForgeApi.Data;
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
        public async Task<List<ClientGetRequest>> GetAll(int owner)
        {
            var clients = await _dbContext.Client.Select(c => new ClientGetRequest
            {
                    Id = c.Id,
                    AddressId = c.AddressId,
                    Owner = c.Owner,
                    Type = c.Type,
                    ClientName = c.ClientName,
                    IN = c.IN,
                    TIN = c.TIN,
                    Mobil = c.Mobil,
                    Tel = c.Tel,
                    Email = c.Email,
                    Address = new AddressGetRequest
                    {
                        Id = c.Address.Id,
                        Owner = c.Address.Owner,
                        Street = c.Address.Street,
                        StreetNumber = c.Address.StreetNumber,
                        City = c.Address.City,
                        PostalCode = c.Address.PostalCode,
                        Country = new CountryGetRequest
                        {
                            Id = c.Address.Country.Id,
                            Value = c.Address.Country.Value,
                            Shortcut = c.Address.Country.Shortcut
                        }
                    }
                }
            )
            .Where(c => c.Owner == owner).ToListAsync();
            if(clients.Count == 0)
            {
                throw new DatabaseCallError("Clients does not exist");
            }
            return clients;


        }
        public async Task<ClientGetRequest> GetById(int clientId)
        {
            var client = await _dbContext.Client.Select(c => new ClientGetRequest
            {
                Id = c.Id,
                AddressId = c.AddressId,
                Owner = c.Owner,
                Type = c.Type,
                ClientName = c.ClientName,
                IN = c.IN,
                TIN = c.TIN,
                Mobil = c.Mobil,
                Tel = c.Tel,
                Email = c.Email,
                Address = new AddressGetRequest
                {
                    Id = c.Address.Id,
                    Owner = c.Address.Owner,
                    Street = c.Address.Street,
                    StreetNumber = c.Address.StreetNumber,
                    City = c.Address.City,
                    PostalCode = c.Address.PostalCode,
                    Country = new CountryGetRequest
                    {
                        Id = c.Address.Country.Id,
                        Value = c.Address.Country.Value,
                        Shortcut = c.Address.Country.Shortcut
                    }
                }
            }
            ).Where(c => c.Id == clientId).ToListAsync();
            if (client.Count == 0)
            {
                throw new DatabaseCallError("Clients does not exist");
            }
            if (client.Count > 1)
            {
                throw new DatabaseCallError("Something unexpected happened. There are more than one client with this ID.");
            }

            return client[0];
        }
        public async Task<bool> Add(int userId ,ClientAddRequest client)
        {
            if(client is null)
            {
                throw new ValidationError("Client is not provided.");
            }

            var newClient = new Client
            {
                AddressId = client.AddressId,
                Owner = userId,
                Type = client.Type,
                ClientName = client.ClientName,
                IN = client.IN,
                TIN = client.TIN,
                Mobil = client.Mobil,
                Tel = client.Tel,
                Email = client.Email
            };
            await _dbContext.Client.AddAsync(newClient);
            await Save();
            return true;
        }
        public async Task<bool> Update(int clientId, ClientUpdateRequest client)
        {
            if (client is null)
            {
                throw new ValidationError("Client is not provided.");
            }

            var localClient = await Get(clientId);

            if (localClient is null)
            {
                throw new DatabaseCallError("Client is not in database.");
            }

            if (client.AddressId is not null)
            {
                var addressControl = await _dbContext.Address.FindAsync(client.AddressId);
                
                if (addressControl is null)
                {
                    throw new ValidationError("Provided address is not in database.");
                }
                if (addressControl.Owner != localClient.Owner)
                {
                    throw new ValidationError("Provided address is not in your possession.");
                }
            }

            localClient.AddressId = client.AddressId ?? localClient.AddressId;
            localClient.Type = client.Type ?? localClient.Type;
            localClient.ClientName = client.ClientName ?? localClient.ClientName;
            localClient.IN = client.IN ?? localClient.IN;
            localClient.TIN = client.TIN ?? localClient.TIN;
            localClient.Mobil = client.Mobil ?? localClient.Mobil;
            localClient.Tel = client.Tel ?? localClient.Tel;
            localClient.Email = client.Email ?? localClient.Email;

            await Save();
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var client = await Get(id);

            if (client is null) 
            {
                throw new DatabaseCallError("Client is not in database.");    
            }

            _dbContext.Client.Remove(client);
            await Save();
            return true;
        }
        private async Task Save()
        {
            int save = await _dbContext.SaveChangesAsync();
            if(!(save > 0))
            {
                throw new DatabaseCallError("Client saving failed.");
            }
        }
        private async Task<Client?> Get(int id)
        {
            return await _dbContext.Client.FindAsync(id);
        }
    }
}
