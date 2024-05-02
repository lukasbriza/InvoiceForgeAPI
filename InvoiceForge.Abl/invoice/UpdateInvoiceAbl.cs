using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
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
                    User isUser = await IsInDatabase<User>(invoice.Owner, "Invalid user Id.");
                    Invoice isInvoice = await IsInDatabase<Invoice>(invoiceId, "Invalid invoice Id.");
                    if(isUser.Id != isInvoice.Owner) throw new ValidationError("Provided invoice is not in your possession.");

                    bool invoiceUpdate = await _repository.Invoice.Update(invoiceId, invoice);
                    if (!invoiceUpdate) throw new ValidationError("Invoice update failed.");

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