using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceAddressCopyRepository: 
        RepositoryExtended<InvoiceAddressCopy, InvoiceAddressCopyAddRequest>,
        IInvoiceAddressCopyRepository
    {
        public InvoiceAddressCopyRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}

        public async Task<InvoiceAddressCopyGetRequest?> GetById(int invoiceAddressCopyId, bool? plain = false)
        {
            DbSet<InvoiceAddressCopy> invoiceAddressCopy = _dbContext.InvoiceAddressCopy;
            if (plain == false)
            {
                await invoiceAddressCopy.Include(i => i.CountryId).LoadAsync();
            }
            var invoiceAddressCopyCall = await invoiceAddressCopy.FindAsync(invoiceAddressCopyId);
            var invoiceAddressCopyResult = new InvoiceAddressCopyGetRequest(invoiceAddressCopyCall, plain);
            return invoiceAddressCopyCall is not null ? invoiceAddressCopyResult : null;
        }
    }
}