using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceTemplateRepository: IInvoiceTemplateRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public InvoiceTemplateRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        private async Task<InvoiceTemplate?> Get(int id)
        {
            return await _dbContext.InvoiceTemplate.FindAsync(id);
        }
        public async Task<List<InvoiceTemplate>?> GetByCondition(Expression<Func<InvoiceTemplate,bool>> condition)
        {
            var result = await _dbContext.InvoiceTemplate.Where(condition).ToListAsync();
            return result;
        }
    }
}