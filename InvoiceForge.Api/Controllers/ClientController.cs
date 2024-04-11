using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.Enum;
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
            return await _clientRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<ClientGetRequest>?> GetPlainAllClients(int userId)
        {
            return await _clientRepository.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{clientId}")]
        public async Task<ClientGetRequest?> GetByClientId(int clientId)
        {
            return await _clientRepository.GetById(clientId);

        }
        [HttpGet]
        [Route("plain/{clientId}")]
        public async Task<ClientGetRequest?> GetPlainByClientId(int clientId)
        {
            return await _clientRepository.GetById(clientId, true);

        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddClient(int userId, ClientAddRequest client)
        {
            if (client is null) throw new ValidationError("Client is not provided.");
            
            var isValidOwner = await _userRepository.GetById(userId, true);
            if (isValidOwner is null) throw new ValidationError("Something unexpected happened. Provided invalid user.");

            var addressValidation = await _addressRepository.GetById(client.AddressId, true);
            if (addressValidation is null) throw new ValidationError("Provided AddressId is invalid.");
            if (addressValidation.Owner != userId) throw new ValidationError("Provided address is not in your possession.");
            
            var clientTypeValidation = _codeListRepository.GetClientTypeById(client.TypeId);
            if (clientTypeValidation is null) throw new ValidationError("Provided wrong TypeId.");
            
            var addClientId = await _clientRepository.Add(userId, client, (ClientType)clientTypeValidation);
            var addClientResult = addClientId is not null;
            if (addClientResult) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return addClientResult;
        }
        [HttpPut]
        [Route("{clientId}")]
        public async Task<bool> UpdateClient(int clientId, ClientUpdateRequest client)
        {
            if (client is null) throw new ValidationError("Client is not provided.");
            
            var user = await _userRepository.GetById(client.Owner);
            if (user is null) return false;

            var isOwnerOfClient = user.Clients?.Where(c => c.Id == clientId);
            if (isOwnerOfClient is null || isOwnerOfClient.Count() != 1) throw new ValidationError("Client is not in your possession.");
            
            var clientValidation = await _clientRepository.GetById(clientId);
            if (clientValidation is null) throw new ValidationError("Client is not in database.");

            if (client.AddressId is not null)
            {
                var addressValidation = await _addressRepository.GetById((int)client.AddressId);
                if (addressValidation is null) throw new ValidationError("Provided AddresId is not in database.");
                if (addressValidation.Owner != user.Id) throw new ValidationError("Provided address is not in your possession.");
            }

            var clientType = client.TypeId is not null ? _codeListRepository.GetClientTypeById((int)client.TypeId) : null;
            if (clientType is null && client.TypeId is not null) throw new ValidationError("Provided client type does not exist in database.");
            
            var clientUpdate = await _clientRepository.Update(clientId, client, clientType);
            
            if (clientUpdate) {
                //OUTDATE LINKED INVOICES
                var invoices = await _invoiceRepository.GetByCondition(i => i.ClientLocal.Id == clientId && i.Owner == user.Id && i.Outdated == false);
                if (invoices is not null && invoices.Count > 0){
                    invoices.ConvertAll(i => {
                        i.Outdated = true;
                        return i;
                    });
                }
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return clientUpdate;
        }
        [HttpDelete]
        [Route("{clientId}")]
        public async Task<bool> DeleteClient(int clientId)
        {
            var hasInvoiceTemplatesReference = await _invoiceTemplateRepository.GetByCondition((template) => template.ClientId == clientId);
            if (hasInvoiceTemplatesReference is not null && hasInvoiceTemplatesReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteClient = await _clientRepository.Delete(clientId);

            if (deleteClient) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return deleteClient;
        }
    }
}
