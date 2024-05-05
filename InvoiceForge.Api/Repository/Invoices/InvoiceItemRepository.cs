using InvoiceForgeApi.Data;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceItemRepository: 
        RepositoryExtended<InvoiceItem, InvoiceItemAddRequest>, 
        IInvoiceItemRepository
    {
        public InvoiceItemRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}
        public async Task<List<InvoiceItemGetRequest>?> GetAll(int userId, bool? plain = false)
        {
            DbSet<InvoiceItem> invoiceItems = _dbContext.InvoiceItem;
            if (plain == false)
            {
                await invoiceItems.Include(i => i.Tariff).LoadAsync();
            }
            var invoiceItemList = await invoiceItems
                .Where(i => i.Owner == userId)
                .ToListAsync();
            return invoiceItemList.ConvertAll(i => new InvoiceItemGetRequest(i, plain));
        }
        public async Task<InvoiceItemGetRequest?> GetById(int invoiceItemId, bool? plain = false)
        {
            DbSet<InvoiceItem> invoiceItem = _dbContext.InvoiceItem;
            if (plain == false)
            {
                await invoiceItem.Include(i => i.Tariff).LoadAsync();
            }
            var invoiceItemCall = await invoiceItem.FindAsync(invoiceItemId);
            var invoiceItemResult = new InvoiceItemGetRequest(invoiceItemCall, plain);
            return invoiceItemCall is not null ? invoiceItemResult : null;
        }
        public async Task<bool> Update(int invoiceItemId, InvoiceItemUpdateRequest invoiceItem)
        {
            var localInvoiceItem = await Get(invoiceItemId);
            if (localInvoiceItem is null) throw new NoEntityError();

            var localSelect = new { localInvoiceItem.ItemName, localInvoiceItem.TariffId };
            var updateSelect = new { invoiceItem.ItemName, invoiceItem.TariffId };
            if (localSelect.Equals(updateSelect)) throw new EqualEntityError();

            localInvoiceItem.ItemName = invoiceItem.ItemName;
            localInvoiceItem.TariffId = invoiceItem.TariffId;
            
            var update = _dbContext.Update(localInvoiceItem);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int userId, InvoiceItemAddRequest item)
        {
            var isInDatabase = await _dbContext.InvoiceItem.AnyAsync((i) =>
                i.Owner == userId &&
                i.ItemName == item.ItemName &&
                i.TariffId == item.TariffId
            );
            return !isInDatabase;
        }
    }
}