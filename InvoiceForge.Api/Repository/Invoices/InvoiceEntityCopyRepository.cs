using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceEntityCopyRepository: 
        RepositoryExtended<InvoiceEntityCopy, InvoiceEntityCopyAddRequest>,
        IInvoiceEntityCopyRepository
    {
        public InvoiceEntityCopyRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}

        public async Task<InvoiceEntityCopyGetRequest?> GetById(int invoiceEntityCopyId, bool? plain = false)
        {
            DbSet<InvoiceEntityCopy> invoiceEntityCopy = _dbContext.InvoiceEntityCopy;
            if (plain == false)
            {
                await invoiceEntityCopy.Include(i => i.AddressCopy).LoadAsync();
            }
            var invoiceEntityCopyCall = await invoiceEntityCopy.FindAsync(invoiceEntityCopyId);
            var invoiceEntityCopyResult = new InvoiceEntityCopyGetRequest(invoiceEntityCopyCall, plain);
            return invoiceEntityCopyCall is not null ? invoiceEntityCopyResult : null;
        }
        public override async Task<List<InvoiceEntityCopy>?> GetByCondition(Expression<Func<InvoiceEntityCopy,bool>> condition)
        {
            var dbSet = _dbContext.Set<InvoiceEntityCopy>();
            await dbSet.Include(e => e.AddressCopy).LoadAsync();
            return await dbSet.Where(condition).ToListAsync();
        }
    }
}