using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceRepository: 
        RepositoryExtended<Invoice, InvoiceAddRequestRepository>, 
        IInvoiceRepository
    {
        public InvoiceRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}
        public async Task<List<InvoiceGetRequest>?> GetAll(int userId, bool? plain = false)
        {
            DbSet<Invoice> invoices = _dbContext.Invoice;
            if (plain == true)
            {
                invoices
                    .Include(i => i.InvoiceServices)
                    .ThenInclude(i => i.InvoiceItem);
            }

            var invoicesList = await invoices
                .Where(i => i.Owner == userId)
                .Select(i => new InvoiceGetRequest(i, plain))
                .ToListAsync();
            return invoicesList;
        }
        public async Task<InvoiceGetRequest?> GetById(int invoiceId, bool? plain = false)
        {
            DbSet<Invoice> invoice = _dbContext.Invoice;
            if (plain == false){
                invoice
                    .Include(i => i.InvoiceServices)
                    .ThenInclude(i => i.InvoiceItem);
            }

            var invoiceCall = await invoice.FindAsync(invoiceId);
            var invoiceResult = new InvoiceGetRequest(invoiceCall);
            return invoiceCall is not null ? invoiceResult : null;
        }
        public async Task<bool> Update(int invoiceId, InvoiceUpdateRequest invoice)
        {
            var localInvoice = await Get(invoiceId);

            if (localInvoice is null)
            {
                throw new DatabaseCallError("Invoice is not in database.");
            }

            localInvoice.Maturity = invoice.Maturity ?? localInvoice.Maturity;
            localInvoice.Exposure = invoice.Exposure ?? localInvoice.Exposure;
            localInvoice.TaxableTransaction = invoice.TaxableTransaction ?? localInvoice.TaxableTransaction;

            var update = _dbContext.Update(localInvoice);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int userId, InvoiceAddRequestRepository invoice)
        {
            var isInDatabase = await _dbContext.Invoice.AnyAsync((i) =>
               i.Owner == userId &&
               i.TemplateId == invoice.TemplateId &&
               i.InvoiceNumber == invoice.InvoiceNumber &&
               i.OrderNumber == invoice.OrderNumber
            );
            return !isInDatabase;
        }
    }
}