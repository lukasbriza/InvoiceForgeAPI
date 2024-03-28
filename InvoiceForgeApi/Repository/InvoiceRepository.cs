using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceRepository: IInvoiceRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public InvoiceRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
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
        public async Task<bool> Delete(int entityId)
        {
            var invoice = await Get(entityId);

            if (invoice is null)
            {
                throw new DatabaseCallError("Invoice is not in database.");
            }

            var entity = _dbContext.Invoice.Remove(invoice);
            return entity.State == EntityState.Deleted;
        }
        private async Task<Invoice?> Get(int id)
        {
            return await _dbContext.Invoice.FindAsync(id);
        }
        public async Task<List<Invoice>?> GetByCondition(Expression<Func<Invoice,bool>> condition)
        {
            var result = await _dbContext.Invoice.Where(condition).ToListAsync();
            return result;
        }
    }
}