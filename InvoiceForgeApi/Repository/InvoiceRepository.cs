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
            invoices
                .Include(i => i.InvoiceServices)
                .ThenInclude(i => i.InvoiceItem);

            var invoicesList = await invoices
                .Select(i => new InvoiceGetRequest(i))
                .Where(i => i.Owner == userId)
                .ToListAsync();
            return invoicesList;
        }
        public async Task<InvoiceGetRequest?> GetById(int invoiceId, bool? plain = false)
        {
            DbSet<Invoice> invoice = _dbContext.Invoice;
            invoice
                .Include(i => i.InvoiceServices)
                .ThenInclude(i => i.InvoiceItem);

            var invoiceList = await invoice
                .Select(i => new InvoiceGetRequest(i))
                .Where(i => i.Id == invoiceId)
                .ToListAsync();

            if (invoiceList.Count > 1)
            {
                throw new DatabaseCallError("Something unexpected happened. There are more than one invoice with this ID.");
            }

            return invoiceList[0];
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
            return entity.State == EntityState.Added ? entity.Entity.Id : null;

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

            return _dbContext.Entry(localInvoice).State == EntityState.Modified;
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