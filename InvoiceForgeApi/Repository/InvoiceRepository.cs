using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceRepository: RepositoryBase<Invoice>, IInvoiceRepository
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
        public async Task<int?> Add(int userId, InvoiceAddRequestRepository invoice)
        {
            var newInvoice = new Invoice
            {
                Outdated = false,
                Owner = userId,
                TemplateId = invoice.TemplateId,
                NumberingId = invoice.NumberingId,
                InvoiceNumber = invoice.InvoiceNumber,
                OrderNumber = invoice.OrderNumber,
                BasePriceTotal = invoice.BasePriceTotal,
                VATTotal = invoice.VATTotal,
                TotalAll = invoice.TotalAll,
                Currency = invoice.Currency,

                ClientLocal = invoice.ClientLocal,
                ContractorLocal = invoice.ContractorLocal,
                UserAccountLocal = invoice.UserAccountLocal,

                Maturity = invoice.Maturity,
                Exposure = invoice.Exposure,
                TaxableTransaction = invoice.TaxableTransaction,
                Created = invoice.Created
            };
            var entity = await _dbContext.Invoice.AddAsync(newInvoice);
            
            if (entity.State == EntityState.Added) await _dbContext.SaveChangesAsync();
            return entity.State == EntityState.Unchanged ? entity.Entity.Id : null;

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
    }
}