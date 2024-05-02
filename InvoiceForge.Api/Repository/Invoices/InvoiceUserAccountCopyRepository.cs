using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceUserAccountCopyRepository: 
        RepositoryExtended<InvoiceUserAccountCopy, InvoiceUserAccountCopyAddRequest>,
        IInvoiceUserAccountCopyRepository
    {
        public InvoiceUserAccountCopyRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}

        public async Task<InvoiceUserAccountCopyGetRequest?> GetById(int invoiceUserAccountCopyId, bool? plain = false)
        {
            DbSet<InvoiceUserAccountCopy> invoiceUserAccountCopy = _dbContext.InvoiceUserAccountCopy;
            if (plain == false)
            {
                await invoiceUserAccountCopy.Include(i => i.Bank).LoadAsync();
            }
            var invoiceUserAccountCopyCall = await invoiceUserAccountCopy.FindAsync(invoiceUserAccountCopyId);
            var invoiceUserAccountCopyResult = new InvoiceUserAccountCopyGetRequest(invoiceUserAccountCopyCall, plain);
            return invoiceUserAccountCopyCall is not null ? invoiceUserAccountCopyResult : null;
        }
    }
}