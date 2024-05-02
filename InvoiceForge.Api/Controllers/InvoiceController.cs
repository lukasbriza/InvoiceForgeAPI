using InvoiceForgeApi.Abl.invoice;
using InvoiceForgeApi.DTO;
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
        public async Task<List<InvoiceGetRequest>?> GetAllInvoices(int userId)
        {
            return await _repository.Invoice.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<InvoiceGetRequest>?> GetPlainAllInvoices(int userId)
        {
            return await _repository.Invoice.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{invoiceId}")]
        public async Task<InvoiceGetRequest?> GetInvoiceById(int invoiceId)
        {
            return await _repository.Invoice.GetById(invoiceId);
        }
        [HttpGet]
        [Route("plain/{invoiceId}")]
        public async Task<InvoiceGetRequest?> GetPlainInvoiceById (int invoiceId)
        {
            return await _repository.Invoice.GetById(invoiceId, true);
        }
        [HttpPost]
        [Route("generate/{userId}")]
        public async Task<bool> GenerateInvoice(int userId, InvoiceAddRequest invoice)
        {
            if (invoice is null) throw new ValidationError("Invoice is not provided.");
            var abl = new GenerateInvoiceAbl(_repository);
            var result = await abl.Resolve(userId, invoice);
            return result;
        }
        [HttpPut]
        [Route("{invoiceId}")]
        public async Task<bool> UpdateInvoice(int invoiceId, InvoiceUpdateRequest invoice)
        {
            if (invoice is null) throw new ValidationError("Invoice is not provided.");
            var abl = new UpdateInvoiceAbl(_repository);
            var result = await abl.Resolve(invoiceId, invoice);
            return result;

        }
        [HttpDelete]
        [Route("{invoiceId}")]
        public async Task<bool> DeleteInvoice(int invoiceId)
        {
            var abl = new DeleteInvoiceAbl(_repository);
            var result = await abl.Resolve(invoiceId);
            return result;
        }
    }
}