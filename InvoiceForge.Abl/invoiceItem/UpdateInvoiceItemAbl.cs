using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
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
                User isUser = await IsInDatabase<User>(invoiceItem.Owner);
                InvoiceItem isInvoiceItem = await IsInDatabase<InvoiceItem>(invoiceItemId);
                if (isUser.Id != isInvoiceItem.Owner) throw new NoPossessionError();

                List<InvoiceService>? invoiceServiceReferences = await _repository.InvoiceService.GetByCondition(s => s.InvoiceItemId == invoiceItemId);
                if (invoiceServiceReferences is not null && invoiceServiceReferences.Any()) throw new EntityReferenceError();

                await IsInDatabase<Tariff>(invoiceItem.TariffId);

                var isInvoiceItemNameDuplicit = await _repository.InvoiceItem.GetByCondition(i => i.ItemName == invoiceItem.ItemName && i.Owner == isUser.Id);
                if (isInvoiceItemNameDuplicit is not null && isInvoiceItemNameDuplicit.Any()) throw new NotUniqueEntityError("Item name");

                bool updateInvoiceItem = await _repository.InvoiceItem.Update(invoiceItemId, invoiceItem);

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