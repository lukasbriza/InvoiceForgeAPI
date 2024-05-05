using InvoiceForgeApi.Abl.invoiceTemplate;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
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
        public async Task<CustomResponse<List<InvoiceTemplateGetRequest>>> GetAllTemplates(int userId)
        {
            var task = await _repository.InvoiceTemplate.GetAll(userId);
            return CreateRepsonse(task ?? new List<InvoiceTemplateGetRequest>());
        }  
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<CustomResponse<List<InvoiceTemplateGetRequest>>> GetPlainAllTemplates(int userId)
        {
            var task = await _repository.InvoiceTemplate.GetAll(userId, true);
            return CreateRepsonse(task ?? new List<InvoiceTemplateGetRequest>());
        }
        [HttpGet]
        [Route("{templateId}")]
        public async Task<CustomResponse<InvoiceTemplateGetRequest?>> GetByTemplateId(int templateId)
        {
            var task = await _repository.InvoiceTemplate.GetById(templateId);
            return CreateRepsonse(task);
        } 
        [HttpGet]
        [Route("plain/{templateId}")]
        public async Task<CustomResponse<InvoiceTemplateGetRequest?>> GetPlainByTemplateId(int templateId)
        {
            var task = await _repository.InvoiceTemplate.GetById(templateId, true);
            return CreateRepsonse(task);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<CustomResponse<bool>> AddTemplate(int userId, InvoiceTemplateAddRequest template)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new AddInvoiceTemplateAbl(_repository);
            var result = await abl.Resolve(userId, template);
            return CreateRepsonse(result);
            
        }
        [HttpPut]
        [Route("{templateId}")]
        public async Task<CustomResponse<bool>> UpdateTemplate(int templateId, InvoiceTemplateUpdateRequest template)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new UpdateInvoiceTemplateAbl(_repository);
            var result = await abl.Resolve(templateId, template);
            return CreateRepsonse(result);
        } 
        [HttpDelete]
        [Route("{templateId}")]
        public async Task<CustomResponse<bool>> DeleteTemplate(int templateId)
        {
            var abl = new DeleteInvoiceTemplateAbl(_repository);
            var result = await abl.Resolve(templateId);
            return CreateRepsonse(result);
        }
    }
}