using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryWrapper _repository;
        public ClientController(IRepositoryWrapper repository)
        {
            _clientRepository = repository.Client;
            _userRepository = repository.User;
            _repository = repository;
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<ClientGetRequest>?> GetAllClients(int userId)
        {
            return await _clientRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("{clientId}")]
        public async Task<ClientGetRequest?> GetByClientId(int clientId)
        {
            return await _clientRepository.GetById(clientId);

        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddClient(int userId, ClientAddRequest client)
        {
            var isValidOwner = await _userRepository.GetById(userId);
            if(isValidOwner is null) return false;
            
            var addClient = await _clientRepository.Add(userId, client);
            if (addClient) await _repository.Save();
            return addClient;
        }
        [HttpPut]
        [Route("{clientId}")]
        public async Task<bool> UpdateClient(int clientId, ClientUpdateRequest client)
        {
            var user = await _userRepository.GetById(client.Owner);
            if (user is null) return false;

            var isOwnerOfClient = user.Clients.Where(c => c.Id == clientId);
            if(isOwnerOfClient is null || isOwnerOfClient.Count() != 1)
            {
                throw new ValidationError("Provided values are wrong.");
            }

            var clientUpdate = await _clientRepository.Update(clientId, client);
            if (clientUpdate) await _repository.Save();
            return clientUpdate;
        }
        [HttpDelete]
        [Route("{clientId}")]
        public async Task<bool> DeleteClient(int clientId)
        {
            var deleteClient = await _clientRepository.Delete(clientId);
            if (deleteClient) await _repository.Save();
            return deleteClient;
        }
    }
}
