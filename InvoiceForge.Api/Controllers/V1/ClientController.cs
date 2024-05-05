using InvoiceForgeApi.Abl.client;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
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
        public async Task<CustomResponse<List<ClientGetRequest>>> GetAllClients(int userId)
        {
            var task = await _repository.Client.GetAll(userId);
            return CreateRepsonse(task ?? new List<ClientGetRequest>());
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<CustomResponse<List<ClientGetRequest>>> GetPlainAllClients(int userId)
        {
            var task = await _repository.Client.GetAll(userId, true);
            return CreateRepsonse(task ?? new List<ClientGetRequest>());
        }
        [HttpGet]
        [Route("{clientId}")]
        public async Task<CustomResponse<ClientGetRequest?>> GetByClientId(int clientId)
        {
            var task = await _repository.Client.GetById(clientId);
            return CreateRepsonse(task);

        }
        [HttpGet]
        [Route("plain/{clientId}")]
        public async Task<CustomResponse<ClientGetRequest?>> GetPlainByClientId(int clientId)
        {
            var task = await _repository.Client.GetById(clientId, true);
            return CreateRepsonse(task);

        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<CustomResponse<bool>> AddClient(int userId, ClientAddRequest client)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new AddClientAbl(_repository);
            var result = await abl.Resolve(userId, client);
            return CreateRepsonse(result);
        }
        [HttpPut]
        [Route("{clientId}")]
        public async Task<CustomResponse<bool>> UpdateClient(int clientId, ClientUpdateRequest client)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new UpdateClientAbl(_repository);
            var result = await abl.Resolve(clientId, client);
            return CreateRepsonse(result);
            
        }
        [HttpDelete]
        [Route("{clientId}")]
        public async Task<CustomResponse<bool>> DeleteClient(int clientId)
        {
            var abl = new DeleteClientAbl(_repository);
            var result = await abl.Resolve(clientId);
            return CreateRepsonse(result);
        }
    }
}
