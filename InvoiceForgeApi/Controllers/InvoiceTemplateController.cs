using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/contractor")]
    public class InvoiceTemplateController : ControllerBase
    {
        private readonly IInvoiceTemplateRepository _invoiceTemplateRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IContractorRepository _contractorRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryWrapper _repository;

        public InvoiceTemplateController(IRepositoryWrapper repository)
        {
            _repository = repository;
            _invoiceTemplateRepository = repository.InvoiceTemplate;
            _addressRepository = repository.Address;
            _contractorRepository = repository.Contractor;
            _userAccountRepository = repository.UserAccount;
            _userRepository = repository.User;
            _clientRepository = repository.Client;

        }
        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<InvoiceTemplateGetRequest>?> GetAllTemplates(int userId)
        {
            return await _invoiceTemplateRepository.GetAll(userId);
        }  
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<InvoiceTemplateGetRequest>?> GetPlainAllTemplates(int userId)
        {
            return await _invoiceTemplateRepository.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{templateId}")]
        public async Task<InvoiceTemplateGetRequest?> GetByTemplateId(int templateId)
        {
            return await _invoiceTemplateRepository.GetById(templateId);
        } 
        [HttpGet]
        [Route("plain/{templateId}")]
        public async Task<InvoiceTemplateGetRequest?> GetPlainByTemplateId(int templateId)
        {
            return await _invoiceTemplateRepository.GetById(templateId, true);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddTemplate(int userId, InvoiceTemplateAddRequest template)
        {
            if (template is null) throw new ValidationError("Template is not provided.");

            var clientValidation = await _clientRepository.GetById(template.ClientId, true);
            if(clientValidation is null) throw new ValidationError("Provided ClientId is invalid.");
            if (clientValidation.Owner != userId) throw new ValidationError("Provided client is not in your possession.");

            var contractorValidation = await _contractorRepository.GetById(template.ContractorId, true);
            if (contractorValidation is null) throw new ValidationError("Provided ContractorId is invalid.");
            if (contractorValidation.Owner != userId) throw new ValidationError("Provided contractor is not in your possession.");

            var userAccountValidation = await _userAccountRepository.GetById(template.UserAccountId, true);
            if (userAccountValidation is null) throw new ValidationError("Provided UserAccountId is invalid.");
            if (userAccountValidation.Owner != userId) throw new ValidationError("Provided user account is not in your possession.");

            var isValidOwner = await _userAccountRepository.GetById(userId, true);
            if (isValidOwner is null) throw new ValidationError("Something unexpected happened. Provided invalid user.");

            var addTemplate = await _invoiceTemplateRepository.Add(userId, template);
            if (addTemplate) await _repository.Save();
            return addTemplate;
        }
        [HttpPut]
        [Route("{templateId}")]
        public async Task<bool> UpdateTemplate(int templateId, InvoiceTemplateUpdateRequest template)
        {
            if (template is null) throw new ValidationError("Template is not provided.");
            
            var isUniqueName = await _invoiceTemplateRepository.GetByCondition(t => t.TemplateName == template.TemplateName);
            if (isUniqueName is not null || isUniqueName?.Count > 0) throw new ValidationError("Teplate name must be unique.");

            var user = await _userRepository.GetById(template.Owner);
            if (user is null) return false;

            var isOwnerOfTemplate = user.InvoiceTemplates.Where(t => t.Id == templateId);
            if (isOwnerOfTemplate is null || isOwnerOfTemplate.Count() != 1) throw new ValidationError("Template is not in your possession.");

            if (template.ClientId is not null) {
                var clientValidation = await _clientRepository.GetById((int)template.ClientId, true);
                if(clientValidation is null) throw new ValidationError("Provided ClientId is invalid.");
                if (clientValidation.Owner != template.Owner) throw new ValidationError("Provided client is not in your possession.");
            }

            if (template.ContractorId is not null)
            {
                var contractorValidation = await _contractorRepository.GetById((int)template.ContractorId, true);
                if (contractorValidation is null) throw new ValidationError("Provided ContractorId is invalid.");
                if (contractorValidation.Owner != template.Owner) throw new ValidationError("Provided contractor is not in your possession.");
            }

            if (template.UserAccountId is not null)
            {
                var userAccountValidation = await _userAccountRepository.GetById((int)template.UserAccountId, true);
                if (userAccountValidation is null) throw new ValidationError("Provided UserAccountId is invalid.");
                if (userAccountValidation.Owner != template.Owner) throw new ValidationError("Provided user account is not in your possession."); 
            }

            var templateUpdate = await _invoiceTemplateRepository.Update(templateId, template);
            if (templateUpdate) await _repository.Save();
            return templateUpdate;
        } 
        [HttpDelete]
        [Route("{templateId}")]
        public async Task<bool> DeleteTemplate(int template)
        {
            var deleteTemplate = await _invoiceTemplateRepository.Delete(template);
            if (deleteTemplate) await _repository.Save();
            return deleteTemplate;
        }
    }
}