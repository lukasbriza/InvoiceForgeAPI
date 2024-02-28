using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/contractor")]
    public class ContractorController : ControllerBase
    {
        private readonly IContractorRepository _contractorRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICodeListsRepository _codeListsRepository;
        private readonly IInvoiceTemplateRepository _invoiceTemplateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryWrapper _repository;

        public ContractorController(IRepositoryWrapper repository)
        {
            _contractorRepository = repository.Contractor;
            _addressRepository = repository.Address;
            _userRepository = repository.User;
            _codeListsRepository = repository.CodeLists;
            _invoiceTemplateRepository = repository.InvoiceTemplate;
            _repository = repository;
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<ContractorGetRequest>?> GetAllContractors(int userId)
        {
            return await _contractorRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<ContractorGetRequest>?> GetPlainAllContractors(int userId)
        {
            return await _contractorRepository.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{contractorId}")]
        public async Task<ContractorGetRequest?> GetByContractorId(int contractorId)
        {
            return await _contractorRepository.GetById(contractorId);
        }
        [HttpGet]
        [Route("plain/{contractorId}")]
        public async Task<ContractorGetRequest?> GetPlainByContractorId(int contractorId)
        {
            return await _contractorRepository.GetById(contractorId, true);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddContractor(int userId, ContractorAddRequest contractor)
        {
            if (contractor is null) throw new ValidationError("Contractor is not provided.");

            var addressValidation = await _addressRepository.GetById(contractor.AddressId);
            if (addressValidation is null) throw new ValidationError("Provided AddressId is invalid.");
            if (addressValidation.Owner != userId) throw new ValidationError("Provided address is not in your possession.");

            var clientTypeValidation = _codeListsRepository.GetClientTypeById(contractor.TypeId);
            if (clientTypeValidation is null) throw new ValidationError("Provided wrong TypeId.");

            var isValidOwner = await _userRepository.GetById(userId);
            if (isValidOwner is null) throw new ValidationError("Something unexpected happened. Provided invalid user.");

            var addContractor = await _contractorRepository.Add(userId, contractor, (ClientType)clientTypeValidation);
            if (addContractor) await _repository.Save();
            return addContractor; 
        }
        [HttpPut]
        [Route("{contractorId}")]
        public async Task<bool> UpdateContractor(int contractorId, ContractorUpdateRequest contractor)
        {
            if (contractor is null) throw new ValidationError("Contractor is not provided.");

            var user = await _userRepository.GetById(contractor.Owner);
            if (user is null) return false;

            var isOwnerOfContractor = user.Contractors.Where(c => c.Id == contractorId);
            if (isOwnerOfContractor is null || isOwnerOfContractor.Count() != 1) throw new ValidationError("Contractor is not in your possession.");

            var contractorValidation = await _contractorRepository.GetById(contractorId);
            if (contractorValidation is null) throw new ValidationError("Contractor is not in database.");

            if (contractor.AddressId is not null)
            {
                var addressValidation = await _addressRepository.GetById((int)contractor.AddressId);
                if (addressValidation is null) throw new ValidationError("Provided AddressId is not in database.");
                if (addressValidation.Owner != user.Id) throw new ValidationError("Provided address is not in your possession.");
            }

            var clientType = contractor.TypeId is not null ? _codeListsRepository.GetClientTypeById((int)contractor.TypeId) : null;
            if (clientType is null && contractor.TypeId is not null) throw new ValidationError("Provided client type does not exist in database.");
            var contractorUpdate = await _contractorRepository.Update(contractorId,contractor,clientType);
            if (contractorUpdate) await _repository.Save();
            return contractorUpdate;
        }
        [HttpDelete]
        [Route("{contractorId}")]
        public async Task<bool> DeleteContractor(int contractorId)
        {
            var hasInvoiceTemplatesReference = await _invoiceTemplateRepository.GetByCondition((template) => template.ContractorId == contractorId);
            if (hasInvoiceTemplatesReference is not null && hasInvoiceTemplatesReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteContractor = await _contractorRepository.Delete(contractorId);
            if (deleteContractor) await _repository.Save();
            return deleteContractor;
        }
    }
}