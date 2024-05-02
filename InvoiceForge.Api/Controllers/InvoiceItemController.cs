using InvoiceForgeApi.Abl.invoiceItem;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
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
        public async Task<List<InvoiceItemGetRequest>?> GetAllInvoiceItems(int userId)
        {
            return await _repository.InvoiceItem.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<InvoiceItemGetRequest>?> GetPlainAllInvoiceItems(int userId)
        {
            return await _repository.InvoiceItem.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{invoicItemId}")]
        public async Task<InvoiceItemGetRequest?> GetByInvoiceItemId(int invoicItemId)
        {
            return await _repository.InvoiceItem.GetById(invoicItemId);
        }
        [HttpGet]
        [Route("plain/{invoicItemId}")]
        public async Task<InvoiceItemGetRequest?> GetPlainByInvoiceItemId(int invoicItemId)
        {
            return await _repository.InvoiceItem.GetById(invoicItemId, true);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddInvoiceItem(int userId, InvoiceItemAddRequest invoiceItem)
        {
            if (invoiceItem is null) throw new ValidationError("Invoice item is not provided.");
            var abl = new AddInvoiceItemAbl(_repository);
            var result = await abl.Resolve(userId, invoiceItem);
            return result;
        }
        [HttpPut]
        [Route("{invoiceItemId}")]
        public async Task<bool> UpdateInvoiceItem(int invoiceItemId, InvoiceItemUpdateRequest invoiceItem)
        {
            if (invoiceItem is null) throw new ValidationError("Invoice item is not provided.");
            var abl = new UpdateInvoiceItemAbl(_repository);
            var result = await abl.Resolve(invoiceItemId, invoiceItem);
            return result;
        }
        [HttpDelete]
        [Route("{invoiceItemId}")]
        public async Task<bool> DeleteInvoiceItem(int invoiceItemId)
        {
            var abl = new DeleteInvoiceItemAbl(_repository);
            var result = await abl.Resolve(invoiceItemId);
            return result;
        }
    }
}