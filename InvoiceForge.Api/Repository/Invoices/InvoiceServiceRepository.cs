using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public interface IAddRange {
        public Task<bool> Add(int InvoiceId, List<InvoiceServiceExtendedAddRequest> invoiceServices);
    }
    public class InvoiceServiceRepository: 
        RepositoryExtended<InvoiceService, InvoiceServiceExtendedAddRequest>, 
        IInvoiceServiceRepository
    {
        public InvoiceServiceRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}
        public async Task<List<InvoiceServiceGetRequest>?> GetAll(int invoiceId, bool? plain = false)
        {
            var invoiceServices = _dbContext.InvoiceService;
            if (plain == false)
            {
                await invoiceServices
                    .Include(i => i.InvoiceItem)
                    .ThenInclude(i => i!.Tariff)
                    .LoadAsync();
            }
            var invoiceServicesList = await invoiceServices
                .Where(i => i.InvoiceId == invoiceId)
                .ToListAsync();
            return invoiceServicesList.ConvertAll(i => new InvoiceServiceGetRequest(i, plain));
        }
        public async Task<InvoiceServiceGetRequest?> GetById(int itemServiceId, bool? plain = false)
        {
            var invoiceService = _dbContext.InvoiceService;
            if (plain == false)
            {
                await invoiceService
                    .Include(i => i.InvoiceItem)
                    .ThenInclude(i => i!.Tariff)
                    .LoadAsync();
            }
            var invoiceServiceCall = await invoiceService.FindAsync(itemServiceId);
            var invoiceServiceResult = new InvoiceServiceGetRequest(invoiceServiceCall, plain);
            return invoiceServiceCall is not null ? invoiceServiceResult : null;
        }
        public async Task<bool> Add(int InvoiceId, List<InvoiceServiceExtendedAddRequest> invoiceServices)
        {
            IEnumerable<InvoiceService> newInvoiceServices = invoiceServices.Select(i => new InvoiceService
                {
                    InvoiceId = InvoiceId, 
                    InvoiceItemId = i.ItemId,
                    Units = i.Units,
                    PricePerUnit = i.PricePerUnit,
                    BasePrice = i.BasePrice,
                    VAT = i.VAT,
                    Total = i.Total,
                }
            );
            if (newInvoiceServices != null)
            {
                await _dbContext.InvoiceService.AddRangeAsync(newInvoiceServices);
                return true;
            }
            return false;
        }
        public async Task<bool> Update(int invoiceServiceId, InvoiceServiceUpdateRequest invoiceService)
        {
            var localInvoiceservice = await Get(invoiceServiceId);
            if (localInvoiceservice is null) throw new DatabaseCallError("Invoice service is not provided.");

            var localSelect = new { 
                localInvoiceservice.Units, 
                localInvoiceservice.PricePerUnit, 
                localInvoiceservice.InvoiceItemId, 
                localInvoiceservice.BasePrice
            };
            var updateSelect = new {  
                invoiceService.Units,
                invoiceService.PricePerUnit,
                InvoiceItemId = invoiceService.ItemId,
                invoiceService.BasePrice
            };
            if (localSelect.Equals(updateSelect)) throw new ValidationError("One of properties must be different from actual ones.");

            localInvoiceservice.Units = invoiceService.Units;
            localInvoiceservice.PricePerUnit = invoiceService.PricePerUnit;
            localInvoiceservice.InvoiceItemId = invoiceService.ItemId;
            localInvoiceservice.BasePrice = invoiceService.BasePrice;
            
            var update = _dbContext.Update(localInvoiceservice);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int invoiceId, InvoiceServiceExtendedAddRequest service)
        {
            var isInDatabase = await _dbContext.InvoiceService.AnyAsync((s) =>
               s.Units == service.Units &&
               s.PricePerUnit == service.PricePerUnit &&
               s.InvoiceItemId == service.ItemId &&
               s.BasePrice == service.BasePrice &&
               s.VAT == service.VAT &&
               s.Total == service.Total
            );
            return !isInDatabase;
        }
    }
}