using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
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
                await invoices
                    .Include(i => i.InvoiceServices)
                    .ThenInclude(i => i.InvoiceItem)
                    .LoadAsync();
            }

            var invoicesList = await invoices
                .Where(i => i.Owner == userId)
                .ToListAsync();
            return invoicesList.ConvertAll(i => new InvoiceGetRequest(i, plain));
        }
        public async Task<InvoiceGetRequest?> GetById(int invoiceId, bool? plain = false)
        {
            DbSet<Invoice> invoice = _dbContext.Invoice;
            if (plain == false){
                await invoice
                    .Include(i => i.InvoiceServices)
                    .ThenInclude(i => i.InvoiceItem)
                    .LoadAsync();
            }

            var invoiceCall = await invoice.FindAsync(invoiceId);
            var invoiceResult = new InvoiceGetRequest(invoiceCall);
            return invoiceCall is not null ? invoiceResult : null;
        }
        public async Task<bool> Update(int invoiceId, InvoiceUpdateRequest invoice)
        {
            var localInvoice = await Get(invoiceId);
            if (localInvoice is null) throw new DatabaseCallError("Invoice is not in database.");

            var localSelect = new { localInvoice.Maturity, localInvoice.Exposure, localInvoice.TaxableTransaction };
            var updateSelect = new { invoice.Maturity, invoice.Exposure, invoice.TaxableTransaction };
            if (localSelect.Equals(updateSelect)) throw new ValidationError("One of properties must be different from actual ones.");

            localInvoice.Maturity = invoice.Maturity;
            localInvoice.Exposure = invoice.Exposure;
            localInvoice.TaxableTransaction = invoice.TaxableTransaction;

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