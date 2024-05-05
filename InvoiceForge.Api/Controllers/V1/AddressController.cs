using InvoiceForgeApi.Abl.address;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
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
        public async Task<CustomResponse<List<AddressGetRequest>>> GetAllAddresses(int userId)
        {
            var task = await _repository.Address.GetAll(userId);
            return CreateRepsonse(task ?? new List<AddressGetRequest>());
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<CustomResponse<List<AddressGetRequest>>> GetPlainAllAddresses(int userId)
        {
            var task = await _repository.Address.GetAll(userId, true);
            return CreateRepsonse(task ?? new List<AddressGetRequest>());
        }
        [HttpGet]
        [Route("{addressId}")]
        public async Task<CustomResponse<AddressGetRequest?>> GetByAddressId(int addressId)
        {
            var task = await _repository.Address.GetById(addressId);
            return CreateRepsonse(task);
        }
        [HttpGet]
        [Route("plain/{addressId}")]
        public async Task<CustomResponse<AddressGetRequest?>> GetPlainByAddressId(int addressId)
        {
            var task = await _repository.Address.GetById(addressId, true);
            return CreateRepsonse(task);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<CustomResponse<bool>> AddAddress(int userId, AddressAddRequest address)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new AddAddressAbl(_repository);
            var result = await abl.Resolve(userId, address);
            return CreateRepsonse(result);
        }
        [HttpPut]
        [Route("{addressId}")]
        public async Task<CustomResponse<bool>> UpdateAddress(int addressId, AddressUpdateRequest address)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new UpdateAddressAbl(_repository);
            var result = await abl.Resolve(addressId, address);
            return CreateRepsonse(result);
        }
        [HttpDelete]
        [Route("{addressId}")]
        public async Task<CustomResponse<bool>> DeleteAddress(int addressId)
        {
            var abl = new DeleteAddressAbl(_repository);
            var result = await abl.Resolve(addressId);
            return CreateRepsonse(result);
        }

    }
}