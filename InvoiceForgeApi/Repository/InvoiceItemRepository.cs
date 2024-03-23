
using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceItemRepository: IInvoiceItemRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public InvoiceItemRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<InvoiceItemGetRequest>?> GetAll(int userId, bool? plain = false)
        {
            DbSet<InvoiceItem> invoiceItems = _dbContext.InvoiceItem;
            if (plain == false)
            {
                invoiceItems.Include(i => i.Tariff);
            }
            var invoiceItemList = await invoiceItems
                .Select(i => new InvoiceItemGetRequest(i, plain))
                .Where(i => i.Owner == userId)
                .ToListAsync();
            return invoiceItemList;
        }
        public async Task<InvoiceItemGetRequest?> GetById(int invoiceItemId, bool? plain = false)
        {
            DbSet<InvoiceItem> invoiceItem = _dbContext.InvoiceItem;
            if (plain == false)
            {
                invoiceItem.Include(i => i.Tariff);
            }
            var invoiceItemList = await invoiceItem
                .Select(i => new InvoiceItemGetRequest(i, plain))
                .Where(i => i.Id == invoiceItemId)
                .ToListAsync();
            if (invoiceItemList.Count > 1)
            {
                throw new DatabaseCallError("Something unexpected happended. There are more than one invoice item with this ID.");
            }
            return invoiceItemList[0];
        }
        public async Task<int?> Add(int userId, InvoiceItemAddRequest invoiceItem)
        {
            var newInvoiceItem = new InvoiceItem
            {
                Owner = userId,
                ItemName = invoiceItem.ItemName,
                TariffId = invoiceItem.TariffId
            };
            var entity = await _dbContext.InvoiceItem.AddAsync(newInvoiceItem);
            return entity.State == EntityState.Added ? entity.Entity.Id : null;
        }
        public async Task<bool> Update(int invoiceItemId, InvoiceItemUpdateRequest invoiceItem)
        {
            var localInvoiceItem = await Get(invoiceItemId);
            if (localInvoiceItem is null)
            {
                throw new ValidationError("InvoiceItem is not in database.");
            }

            localInvoiceItem.ItemName = invoiceItem.ItemName ?? localInvoiceItem.ItemName;
            localInvoiceItem.TariffId = invoiceItem.TariffId ?? localInvoiceItem.TariffId;
            
            return _dbContext.Entry(localInvoiceItem).State == EntityState.Modified;
        }
        public async Task<bool> Delete(int invoiceItemId)
        {
            var invoiceItem = await Get(invoiceItemId);

            if (invoiceItem is null)
            {
                throw new DatabaseCallError("InvoiceItem is not in databse.");
            }
            var entity = _dbContext.InvoiceItem.Remove(invoiceItem);
            return entity.State == EntityState.Deleted;
        }
        private async Task<InvoiceItem?> Get(int id)
        {
            return await _dbContext.InvoiceItem.FindAsync(id);
        }
        public async Task<List<InvoiceItem>?> GetByCondition(Expression<Func<InvoiceItem,bool>> condition)
        {
            var result = await _dbContext.InvoiceItem.Where(condition).ToListAsync();
            return result;
        }
    }
}