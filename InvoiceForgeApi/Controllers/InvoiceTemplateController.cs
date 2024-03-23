using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/invoice-template")]
    public class InvoiceTemplateController : ControllerBase
    {
        private readonly IInvoiceTemplateRepository _invoiceTemplateRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IContractorRepository _contractorRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICodeListsRepository _codeListRepository;
        private readonly IRepositoryWrapper _repository;

        public InvoiceTemplateController(IRepositoryWrapper repository)
        {
            _repository = repository;
            _invoiceTemplateRepository = repository.InvoiceTemplate;
            _contractorRepository = repository.Contractor;
            _userAccountRepository = repository.UserAccount;
            _invoiceRepository = repository.Invoice;
            _userRepository = repository.User;
            _clientRepository = repository.Client;
            _codeListRepository = repository.CodeLists;

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

            var uniqueTemplateNameValidation = await _invoiceTemplateRepository.GetByCondition(t => t.TemplateName == template.TemplateName && t.Owner == userId);
            if (uniqueTemplateNameValidation is not null && uniqueTemplateNameValidation.Count > 0) throw new ValidationError("Template name must be unique.");

            var addTemplate = await _invoiceTemplateRepository.Add(userId, template);
            var addTemplateResult = addTemplate is not null;

            if (addTemplateResult) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return addTemplateResult;
        }
        [HttpPut]
        [Route("{templateId}")]
        public async Task<bool> UpdateTemplate(int templateId, InvoiceTemplateUpdateRequest template)
        {
            if (template is null) throw new ValidationError("Template is not provided.");

            var user = await _userRepository.GetById(template.Owner);
            if (user is null) return false;

            var isOwnerOfTemplate = user.InvoiceTemplates?.Where(t => t.Id == templateId);
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

            if (template.CurrencyId is not null)
            {
                var currencyValidation = await _codeListRepository.GetCurrencyById((int)template.CurrencyId);
                if (currencyValidation is null) throw new ValidationError("Provided CurrencyId is invalid.");
            }

            if (template.TemplateName is not null)
            {
                var templateNameValidation = await _invoiceTemplateRepository.GetByCondition(t => t.TemplateName == template.TemplateName && t.Owner == user.Id);
                if (templateNameValidation is not null && templateNameValidation.Count > 0) throw new ValidationError("Template name must be unique.");
            }

            var templateUpdate = await _invoiceTemplateRepository.Update(templateId, template);

            if (templateUpdate) {
                //OUTDATE LINKED INVOICES
                var invoices = await _invoiceRepository.GetByCondition(i => i.TemplateId == templateId && i.Owner == user.Id && i.Outdated == false);
                if(invoices is not null && invoices.Count > 0)
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
            return templateUpdate;
        } 
        [HttpDelete]
        [Route("{templateId}")]
        public async Task<bool> DeleteTemplate(int templateId)
        {
            var hasInvoiceReference = await _invoiceRepository.GetByCondition(i => i.TemplateId == templateId);
            if (hasInvoiceReference is not null && hasInvoiceReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteTemplate = await _invoiceTemplateRepository.Delete(templateId);

            if (deleteTemplate) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return deleteTemplate;
        }
    }
}