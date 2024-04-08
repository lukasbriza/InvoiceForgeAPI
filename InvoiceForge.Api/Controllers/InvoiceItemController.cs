using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
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
            return await _invoiceItemRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<InvoiceItemGetRequest>?> GetPlainAllInvoiceItems(int userId)
        {
            return await _invoiceItemRepository.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{invoicItemId}")]
        public async Task<InvoiceItemGetRequest?> GetByInvoiceItemId(int invoicItemId)
        {
            return await _invoiceItemRepository.GetById(invoicItemId);
        }
        [HttpGet]
        [Route("plain/{invoicItemId}")]
        public async Task<InvoiceItemGetRequest?> GetPlainByInvoiceItemId(int invoicItemId)
        {
            return await _invoiceItemRepository.GetById(invoicItemId, true);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddInvoiceItem(int userId, InvoiceItemAddRequest invoiceItem)
        {
            if (invoiceItem is null) throw new ValidationError("Client is not provided.");

            var tariffValidation = await _codeListRepository.GetTariffById(invoiceItem.TariffId);
            if(tariffValidation is null) throw new ValidationError("Provided tariff id is invalid.");

            var invoiceItemNameValidation = await _invoiceItemRepository.GetByCondition(i => i.ItemName == invoiceItem.ItemName && i.Owner == userId);
            if (invoiceItemNameValidation is not null && invoiceItemNameValidation.Count > 0) throw new ValidationError("Invoice item name must be unique.");

            var addInvoiceItem = await _invoiceItemRepository.Add(userId, invoiceItem);
            var addInvoiceItemResult = addInvoiceItem is not null;

            if (addInvoiceItemResult) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return addInvoiceItemResult;
        }
        [HttpPut]
        [Route("{invoiceItemId}")]
        public async Task<bool> UpdateInvoiceItem(int invoiceItemId, InvoiceItemUpdateRequest invoiceItem)
        {
            if (invoiceItem is null) throw new ValidationError("Invoice item is not provided.");

            var user = await _userRepository.GetById(invoiceItemId);
            if (user is null) return false;

            var isOwnerOfInvoiceItem = user.InvoiceItems?.Where(i => i.Id == invoiceItemId);
            if (isOwnerOfInvoiceItem is null || isOwnerOfInvoiceItem.Count() != 1) throw new ValidationError("Invoice item is not in your possession.");

            var hasInvoiceServicesReference = await _invoiceServiceRepository.GetByCondition(s => s.InvoiceItemId == invoiceItemId);
            if (hasInvoiceServicesReference is not null && hasInvoiceServicesReference.Count > 0) throw new ValidationError("Can´t update. Still assigned to some entity.");

            if (invoiceItem.TariffId is not null)
            {
                var tariffValidation = _codeListRepository.GetTariffById((int)invoiceItem.TariffId);
                if (tariffValidation is null) throw new ValidationError("Provided TariffId is not in database.");
            }

            if (invoiceItem.ItemName is not null)
            {
                var invoiceItemValldation = await _invoiceItemRepository.GetByCondition(i => i.ItemName == invoiceItem.ItemName && i.Owner == user.Id);
                if (invoiceItemValldation is not null && invoiceItemValldation.Count > 0) throw new ValidationError("Invoice item name must be unique.");
            }

            var invoiceItemUpdate = await _invoiceItemRepository.Update(invoiceItemId, invoiceItem);
            
            if (invoiceItemUpdate) 
            {
                await _repository.Save();
            } else 
            {
                _repository.DetachChanges();
            };
            return invoiceItemUpdate;
        }
        [HttpDelete]
        [Route("{invoiceItemId}")]
        public async Task<bool> DeleteInvoiceItem(int invoiceItemId)
        {
            var hasInvoiceServicesReference = await _invoiceServiceRepository.GetByCondition(s => s.InvoiceItemId == invoiceItemId);
            if (hasInvoiceServicesReference is not null && hasInvoiceServicesReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteInvoiceItem = await _invoiceItemRepository.Delete(invoiceItemId);
            
            if (deleteInvoiceItem) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return deleteInvoiceItem;
        }
    }
}