using InvoiceForgeApi.Abl.address;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/address")]
    public class AddressController: BaseController
    {
        public AddressController(IRepositoryWrapper repository): base(repository) {}
        
        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<AddressGetRequest>?> GetAllAddresses(int userId)
        {
            return await _repository.Address.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<AddressGetRequest>?> GetPlainAllAddresses(int userId)
        {
            return await _repository.Address.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{addressId}")]
        public async Task<AddressGetRequest?> GetByAddressId(int addressId)
        {
            return await _repository.Address.GetById(addressId);
        }
        [HttpGet]
        [Route("plain/{addressId}")]
        public async Task<AddressGetRequest?> GetPlainByAddressId(int addressId)
        {
            return await _repository.Address.GetById(addressId, true);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddAddress(int userId, AddressAddRequest address)
        {
            if (address is null) throw new ValidationError("Address is not provided.");
            var abl = new AddAddressAbl(_repository);
            var result = await abl.Resolve(userId, address);
            return result;
        }
        [HttpPut]
        [Route("{addressId}")]
        public async Task<bool> UpdateAddress(int addressId, AddressUpdateRequest address)
        {
            if (address is null) throw new ValidationError("Address is not provided.");
            var abl = new UpdateAddressAbl(_repository);
            var result = await abl.Resolve(addressId, address);
            return result;
        }
        [HttpDelete]
        [Route("{addressId}")]
        public async Task<bool> DeleteAddress(int addressId)
        {
            var abl = new DeleteAddressAbl(_repository);
            var result = await abl.Resolve(addressId);
            return result;
        }

    }
}