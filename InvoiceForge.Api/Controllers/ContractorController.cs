using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/contractor")]
    public class ContractorController : BaseController
    {
        public ContractorController(IRepositoryWrapper repository): base(repository) {}

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<ContractorGetRequest>?> GetAllContractors(int userId)
        {
            return await _contractorRepository.GetAll(userId, false);
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

            var clientTypeValidation = _codeListRepository.GetClientTypeById(contractor.TypeId);
            if (clientTypeValidation is null) throw new ValidationError("Provided wrong TypeId.");

            var isValidOwner = await _userRepository.GetById(userId);
            if (isValidOwner is null) throw new ValidationError("Something unexpected happened. Provided invalid user.");

            var addContractor = await _contractorRepository.Add(userId, contractor, (ClientType)clientTypeValidation);
            var addContractorResult = addContractor is not null;

            if (addContractorResult) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return addContractorResult;
        }
        [HttpPut]
        [Route("{contractorId}")]
        public async Task<bool> UpdateContractor(int contractorId, ContractorUpdateRequest contractor)
        {
            if (contractor is null) throw new ValidationError("Contractor is not provided.");

            var user = await _userRepository.GetById(contractor.Owner);
            if (user is null) return false;

            var isOwnerOfContractor = user.Contractors?.Where(c => c.Id == contractorId);
            if (isOwnerOfContractor is null || isOwnerOfContractor.Count() != 1) throw new ValidationError("Contractor is not in your possession.");

            var contractorValidation = await _contractorRepository.GetById(contractorId);
            if (contractorValidation is null) throw new ValidationError("Contractor is not in database.");

            if (contractor.AddressId is not null)
            {
                var addressValidation = await _addressRepository.GetById((int)contractor.AddressId);
                if (addressValidation is null) throw new ValidationError("Provided AddressId is not in database.");
                if (addressValidation.Owner != user.Id) throw new ValidationError("Provided address is not in your possession.");
            }

            var clientType = contractor.TypeId is not null ? _codeListRepository.GetClientTypeById((int)contractor.TypeId) : null;
            if (clientType is null && contractor.TypeId is not null) throw new ValidationError("Provided client type does not exist in database.");
            var contractorUpdate = await _contractorRepository.Update(contractorId, contractor,clientType);
           
            if (contractorUpdate) {
                //OUTDATE LINKED INVOICES
                var invoices = await _invoiceRepository.GetByCondition(i => i.ContractorLocal.Id == contractorId && i.Owner == user.Id && i.Outdated == false);
                if (invoices is not null && invoices.Count > 0)
                {
                    invoices.ConvertAll(i => {
                        i.Outdated = true;
                        return i;
                    });
                }
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return contractorUpdate;
        }
        [HttpDelete]
        [Route("{contractorId}")]
        public async Task<bool> DeleteContractor(int contractorId)
        {
            var hasInvoiceTemplatesReference = await _invoiceTemplateRepository.GetByCondition((template) => template.ContractorId == contractorId);
            if (hasInvoiceTemplatesReference is not null && hasInvoiceTemplatesReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteContractor = await _contractorRepository.Delete(contractorId);
            
            if (deleteContractor) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return deleteContractor;
        }
    }
}