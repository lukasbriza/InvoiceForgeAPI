using InvoiceForgeApi.Abl.invoiceTemplate;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/invoice-template")]
    public class InvoiceTemplateController : BaseController
    {
        public InvoiceTemplateController(IRepositoryWrapper repository): base(repository) {}
        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<InvoiceTemplateGetRequest>?> GetAllTemplates(int userId)
        {
            return await _repository.InvoiceTemplate.GetAll(userId);
        }  
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<InvoiceTemplateGetRequest>?> GetPlainAllTemplates(int userId)
        {
            return await _repository.InvoiceTemplate.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{templateId}")]
        public async Task<InvoiceTemplateGetRequest?> GetByTemplateId(int templateId)
        {
            return await _repository.InvoiceTemplate.GetById(templateId);
        } 
        [HttpGet]
        [Route("plain/{templateId}")]
        public async Task<InvoiceTemplateGetRequest?> GetPlainByTemplateId(int templateId)
        {
            return await _repository.InvoiceTemplate.GetById(templateId, true);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddTemplate(int userId, InvoiceTemplateAddRequest template)
        {
            if (template is null) throw new ValidationError("Template is not provided.");
            var abl = new AddInvoiceTemplateAbl(_repository);
            var result = await abl.Resolve(userId, template);
            return result;
            
        }
        [HttpPut]
        [Route("{templateId}")]
        public async Task<bool> UpdateTemplate(int templateId, InvoiceTemplateUpdateRequest template)
        {
            if (template is null) throw new ValidationError("Template is not provided.");
            var abl = new UpdateInvoiceTemplateAbl(_repository);
            var result = await abl.Resolve(templateId, template);
            return result;
        } 
        [HttpDelete]
        [Route("{templateId}")]
        public async Task<bool> DeleteTemplate(int templateId)
        {
            var abl = new DeleteInvoiceTemplateAbl(_repository);
            var result = await abl.Resolve(templateId);
            return result;
        }
    }
}