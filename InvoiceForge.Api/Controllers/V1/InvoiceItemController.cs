using InvoiceForgeApi.Abl.invoiceItem;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/invoice-item")]
    public class InvoiceItemController : BaseController
    {
        public InvoiceItemController(IRepositoryWrapper repository): base(repository) {}
        [HttpGet]
        [Route("all/{userId}")]
        public async Task<CustomResponse<List<InvoiceItemGetRequest>>> GetAllInvoiceItems(int userId)
        {
            var task = await _repository.InvoiceItem.GetAll(userId);
            return CreateRepsonse(task ?? new List<InvoiceItemGetRequest>());
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<CustomResponse<List<InvoiceItemGetRequest>>> GetPlainAllInvoiceItems(int userId)
        {
            var task = await _repository.InvoiceItem.GetAll(userId, true);
            return CreateRepsonse(task ?? new List<InvoiceItemGetRequest>());
        }
        [HttpGet]
        [Route("{invoicItemId}")]
        public async Task<CustomResponse<InvoiceItemGetRequest?>> GetByInvoiceItemId(int invoicItemId)
        {
            var task = await _repository.InvoiceItem.GetById(invoicItemId);
            return CreateRepsonse(task);
        }
        [HttpGet]
        [Route("plain/{invoicItemId}")]
        public async Task<CustomResponse<InvoiceItemGetRequest?>> GetPlainByInvoiceItemId(int invoicItemId)
        {
            var task = await _repository.InvoiceItem.GetById(invoicItemId, true);
            return CreateRepsonse(task);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<CustomResponse<bool>> AddInvoiceItem(int userId, InvoiceItemAddRequest invoiceItem)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new AddInvoiceItemAbl(_repository);
            var result = await abl.Resolve(userId, invoiceItem);
            return CreateRepsonse(result);
        }
        [HttpPut]
        [Route("{invoiceItemId}")]
        public async Task<CustomResponse<bool>> UpdateInvoiceItem(int invoiceItemId, InvoiceItemUpdateRequest invoiceItem)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new UpdateInvoiceItemAbl(_repository);
            var result = await abl.Resolve(invoiceItemId, invoiceItem);
            return CreateRepsonse(result);
        }
        [HttpDelete]
        [Route("{invoiceItemId}")]
        public async Task<CustomResponse<bool>> DeleteInvoiceItem(int invoiceItemId)
        {
            var abl = new DeleteInvoiceItemAbl(_repository);
            var result = await abl.Resolve(invoiceItemId);
            return CreateRepsonse(result);
        }
    }
}