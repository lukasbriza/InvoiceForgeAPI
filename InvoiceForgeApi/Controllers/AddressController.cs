using System.Diagnostics.Tracing;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController: ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly IContractorRepository _contractorRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICodeListsRepository _codeListRepository;
        private readonly IRepositoryWrapper _repository;
        public AddressController(IRepositoryWrapper repository)
        {
            _addressRepository = repository.Address;
            _userRepository = repository.User;
            _contractorRepository = repository.Contractor;
            _clientRepository = repository.Client;
            _codeListRepository = repository.CodeLists;
            _repository = repository;
        }
        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<AddressGetRequest>?> GetAllAddresses(int userId)
        {
            return await _addressRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("{addressId}")]
        public async Task<AddressGetRequest?> GetByAddressId(int addressId)
        {
            return await _addressRepository.GetById(addressId);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddAddress(int userId, AddressAddRequest address)
        {
            if (address is null) throw new ValidationError("Address is not provided.");

            var countryValidation = await _codeListRepository.GetCountryById(address.CountryId);
            if (countryValidation is null) throw new ValidationError("Invalid country id.");

            var isValidOwner = await _userRepository.GetById(userId);
            if (isValidOwner is null) throw new ValidationError("Something unexpected happened. Provided invalid user.");

            var addAddress = await _addressRepository.Add(userId, address);
            if (addAddress) await _repository.Save();
            return addAddress;
        }
        [HttpPut]
        [Route("{addressId}")]
        public async Task<bool> UpdateAddress(int addressId, AddressUpdateRequest address)
        {
            var user = await _userRepository.GetById(address.Owner);
            if (user is null) return false;

            var isOwnerOfAddress = user.Addresses.Where(a => a.Id == addressId);
            if (isOwnerOfAddress is null || isOwnerOfAddress.Count() != 1) throw new ValidationError("Provided address is not in your possession.");

            if (address.CountryId is not null)
            {
                var isCountryIdValid = _codeListRepository;
            }
            
            var addressUpdate = await _addressRepository.Update(addressId, address);
            if (addressUpdate) await _repository.Save();
            return addressUpdate;
        }
        [HttpDelete]
        [Route("{addressId}")]
        public async Task<bool> DeleteAddress(int addressId)
        {
            var hasContractReference = await _contractorRepository.GetByCondition((contractor) => contractor.AddressId == addressId);
            if (hasContractReference is not null && hasContractReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");
            var hasClientReference = await _clientRepository.GetByCondition((client) => client.AddressId == addressId);
            if (hasClientReference is not null && hasClientReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteAddress = await _addressRepository.Delete(addressId);
            if (deleteAddress) await _repository.Save();
            return deleteAddress;
        }

    }
}