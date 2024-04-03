using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceItemRepository: RepositoryBase<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}
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
            var invoiceItemCall = await invoiceItem.FindAsync(invoiceItemId);
            var invoiceItemResult = new InvoiceItemGetRequest(invoiceItemCall, plain);
            return invoiceItemCall is not null ? invoiceItemResult : null;
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
            
            if (entity.State == EntityState.Added) await _dbContext.SaveChangesAsync();
            return entity.State == EntityState.Unchanged ? entity.Entity.Id : null;
        }
        public async Task<bool> Update(int invoiceItemId, InvoiceItemUpdateRequest invoiceItem)
        {
            var localInvoiceItem = await Get(invoiceItemId);
            if (localInvoiceItem is null)
            {
                throw new DatabaseCallError("InvoiceItem is not in database.");
            }

            localInvoiceItem.ItemName = invoiceItem.ItemName ?? localInvoiceItem.ItemName;
            localInvoiceItem.TariffId = invoiceItem.TariffId ?? localInvoiceItem.TariffId;
            
            var update = _dbContext.Update(localInvoiceItem);
            return update.State == EntityState.Modified;
        }
    }
}