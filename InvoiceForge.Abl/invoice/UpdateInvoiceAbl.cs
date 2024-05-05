using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoice
{
    public class UpdateInvoiceAbl: AblBase
    {
        public UpdateInvoiceAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int invoiceId, InvoiceUpdateRequest invoice)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(invoice.Owner);
                    Invoice isInvoice = await IsInDatabase<Invoice>(invoiceId);
                    if(isUser.Id != isInvoice.Owner) throw new NoPossessionError();

                    bool invoiceUpdate = await _repository.Invoice.Update(invoiceId, invoice);

                    await SaveResult(invoiceUpdate, transaction);
                    return invoiceUpdate;
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