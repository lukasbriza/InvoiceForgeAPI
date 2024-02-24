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
        private readonly IAddressRepository _addressRepository;
        private readonly ICodeListsRepository _codeListsRepository;
        private readonly IRepositoryWrapper _repository;
        public ClientController(IRepositoryWrapper repository)
        {
            _clientRepository = repository.Client;
            _addressRepository = repository.Address;
            _userRepository = repository.User;
            _codeListsRepository = repository.CodeLists;
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
            if (client is null) throw new ValidationError("Client is not provided.");
            
            var addressValidation = await _addressRepository.GetById(client.AddressId);
            if (addressValidation is null) throw new ValidationError("Provided AddressId is invalid.");
            if (addressValidation.Owner != userId) throw new ValidationError("Provided address is not in your possession.");
            
            var clientTypeValidation = _codeListsRepository.GetClientTypeById(client.TypeId);
            if (clientTypeValidation is null) throw new ValidationError("Provided wrong TypeId.");
            
            var isValidOwner = await _userRepository.GetById(userId);
            if (isValidOwner is null) throw new ValidationError("Something unexpected happened. Provided invalid user.");
            
            var addClient = await _clientRepository.Add(userId, client, clientTypeValidation);
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
            if (isOwnerOfClient is null || isOwnerOfClient.Count() != 1) throw new ValidationError("Client is not in your possession.");
            
            var clientValidation = await _clientRepository.GetById(clientId);
            if (clientValidation is null) throw new ValidationError("Client is not in database.");

            if (client.AddressId is not null)
            {
                var addressValidation = await _addressRepository.GetById((int)client.AddressId);
                if (addressValidation is null) throw new ValidationError("Provided AddresId is not in database.");
                if (addressValidation.Owner != user.Id) throw new ValidationError("Provided address is not in your possession.");
            }

            var clientType = client.TypeId is not null ? _codeListsRepository.GetClientTypeById((int)client.TypeId) : null;
            if (clientType is null && client.TypeId is not null) throw new ValidationError("Provided client type does not exist in database.");
            var clientUpdate = await _clientRepository.Update(clientId, client, clientType);
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
