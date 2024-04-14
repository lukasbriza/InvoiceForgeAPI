

using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.CodeLists;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoiceItem
{
    public class UpdateInvoiceItemAbl: AblBase
    {
        public UpdateInvoiceItemAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int invoiceItemId, InvoiceItemUpdateRequest invoiceItem)
        {
           using (var transaction = await _repository.BeginTransaction())
           {
            try
            {
                User isUser = await IsInDatabase<User>(invoiceItem.Owner, "Invalid user Id.");
                InvoiceItem isInvoiceItem = await IsInDatabase<InvoiceItem>(invoiceItemId, "Invalid invoice item Id.");
                if (isUser.Id != isInvoiceItem.Owner) throw new ValidationError("Invoice item is not in your possession.");

                List<InvoiceService>? invoiceServiceReferences = await _repository.InvoiceService.GetByCondition(s => s.InvoiceItemId == invoiceItemId);
                if (invoiceServiceReferences is not null && invoiceServiceReferences.Any()) throw new ValidationError("Can´t update. Still assigned to some entity.");

                if (invoiceItem.TariffId is not null) await IsInDatabase<Tariff>((int)invoiceItem.TariffId, "Invalid tariff Id.");
                if (invoiceItem.ItemName is not null)
                {
                    var isInvoiceItemNameDuplicit = await _repository.InvoiceItem.GetByCondition(i => i.ItemName == invoiceItem.ItemName && i.Owner == isUser.Id);
                    if (isInvoiceItemNameDuplicit is not null && isInvoiceItemNameDuplicit.Any()) throw new ValidationError("Invoice item name must be unique.");
                }

                bool updateInvoiceItem = await _repository.InvoiceItem.Update(invoiceItemId, invoiceItem);
                if (updateInvoiceItem) throw new ValidationError("Invoice item update failed.");

                await SaveResult(updateInvoiceItem, transaction);
                return updateInvoiceItem;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
           } 
        }
    }
}