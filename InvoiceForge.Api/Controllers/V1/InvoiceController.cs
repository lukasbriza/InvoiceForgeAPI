using InvoiceForgeApi.Abl.invoice;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/invoice")]
    public class InvoiceController : BaseController
    {

        public InvoiceController(IRepositoryWrapper repository): base(repository) {}

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<CustomResponse<List<InvoiceGetRequest>>> GetAllInvoices(int userId)
        {
            var task = await _repository.Invoice.GetAll(userId);
            return CreateRepsonse(task ?? new List<InvoiceGetRequest>());
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<CustomResponse<List<InvoiceGetRequest>>> GetPlainAllInvoices(int userId)
        {
            var task = await _repository.Invoice.GetAll(userId, true);
            return CreateRepsonse(task ?? new List<InvoiceGetRequest>());
        }
        [HttpGet]
        [Route("{invoiceId}")]
        public async Task<CustomResponse<InvoiceGetRequest?>> GetInvoiceById(int invoiceId)
        {
            var task = await _repository.Invoice.GetById(invoiceId);
            return CreateRepsonse(task);
        }
        [HttpGet]
        [Route("plain/{invoiceId}")]
        public async Task<CustomResponse<InvoiceGetRequest?>> GetPlainInvoiceById (int invoiceId)
        {
            var task = await _repository.Invoice.GetById(invoiceId, true);
            return CreateRepsonse(task);
        }
        [HttpPost]
        [Route("generate/{userId}")]
        public async Task<CustomResponse<bool>> GenerateInvoice(int userId, InvoiceAddRequest invoice)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new GenerateInvoiceAbl(_repository);
            var result = await abl.Resolve(userId, invoice);
            return CreateRepsonse(result);
        }
        [HttpPut]
        [Route("{invoiceId}")]
        public async Task<CustomResponse<bool>> UpdateInvoice(int invoiceId, InvoiceUpdateRequest invoice)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new UpdateInvoiceAbl(_repository);
            var result = await abl.Resolve(invoiceId, invoice);
            return CreateRepsonse(result);

        }
        [HttpDelete]
        [Route("{invoiceId}")]
        public async Task<CustomResponse<bool>> DeleteInvoice(int invoiceId)
        {
            var abl = new DeleteInvoiceAbl(_repository);
            var result = await abl.Resolve(invoiceId);
            return CreateRepsonse(result);
        }
    }
}