using InvoiceForgeApi.Abl.client;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/client")]
    public class ClientController : BaseController
    {
        public ClientController(IRepositoryWrapper repository): base(repository) {}

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<ClientGetRequest>?> GetAllClients(int userId)
        {
            return await _repository.Client.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<ClientGetRequest>?> GetPlainAllClients(int userId)
        {
            return await _repository.Client.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{clientId}")]
        public async Task<ClientGetRequest?> GetByClientId(int clientId)
        {
            return await _repository.Client.GetById(clientId);

        }
        [HttpGet]
        [Route("plain/{clientId}")]
        public async Task<ClientGetRequest?> GetPlainByClientId(int clientId)
        {
            return await _repository.Client.GetById(clientId, true);

        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddClient(int userId, ClientAddRequest client)
        {
            if (client is null) throw new ValidationError("Client is not provided.");
            var abl = new AddClientAbl(_repository);
            var result = await abl.Resolve(userId, client);
            return result;
        }
        [HttpPut]
        [Route("{clientId}")]
        public async Task<bool> UpdateClient(int clientId, ClientUpdateRequest client)
        {
            if (client is null) throw new ValidationError("Client is not provided.");
            var abl = new UpdateClientAbl(_repository);
            var result = await abl.Resolve(clientId, client);
            return result;
            
        }
        [HttpDelete]
        [Route("{clientId}")]
        public async Task<bool> DeleteClient(int clientId)
        {
            var abl = new DeleteClientAbl(_repository);
            var result = await abl.Resolve(clientId);
            return result;
        }
    }
}
