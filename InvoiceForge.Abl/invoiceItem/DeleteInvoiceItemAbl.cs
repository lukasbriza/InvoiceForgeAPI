using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoiceItem
{
    public class DeleteInvoiceItemAbl: AblBase
    {
        public DeleteInvoiceItemAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int invoiceItemId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    List<InvoiceService>? invoiceServiceReferences = await _repository.InvoiceService.GetByCondition(s => s.InvoiceItemId == invoiceItemId);
                    if (invoiceServiceReferences is not null && invoiceServiceReferences.Any()) throw new EntityReferenceError();

                    bool deleteInvoiceItem = await _repository.InvoiceItem.Delete(invoiceItemId);
                    await SaveResult(deleteInvoiceItem, transaction);
                    return deleteInvoiceItem;
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