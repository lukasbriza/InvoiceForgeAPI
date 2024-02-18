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
        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<ClientGetRequest>> GetAll(int userId)
        {
            return await _clientRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("{clientId}")]
        public async Task<ClientGetRequest> GetById(int clientId)
        {
            return await _clientRepository.GetById(clientId);

        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> Add(int userId, ClientAddRequest client)
        {
            return await _clientRepository.Add(userId, client);
        }
        [HttpPut]
        [Route("{clientId}")]
        public async Task<bool> Update(int clientId, ClientUpdateRequest client)
        {
            return await _clientRepository.Update(clientId, client);
        }
        [HttpDelete]
        [Route("{clientId}")]
        public async Task<bool> Delete(int clientId)
        {
            return await _clientRepository.Delete(clientId);
        }
    }
}
