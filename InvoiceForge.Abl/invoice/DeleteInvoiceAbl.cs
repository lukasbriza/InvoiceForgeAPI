using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoice
{
    public class DeleteInvoiceAbl: AblBase
    {
        public DeleteInvoiceAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int invoiceId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    Invoice isInvoice = await IsInDatabase<Invoice>(invoiceId);
                    List<InvoiceService>? services = await _repository.InvoiceService.GetByCondition(s => s.InvoiceId == invoiceId);
                    services?.ForEach(async s => {
                        bool deleteService = await _repository.InvoiceService.Delete(s.Id);
                        if (!deleteService) throw new OperationError("Removing invoice service failed.");
                    });

                    bool invoiceDelete = await _repository.Invoice.Delete(invoiceId);
                    await SaveResult(invoiceDelete, transaction);
                    return invoiceDelete;
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