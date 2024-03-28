using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceServiceRepository: IInvoiceServiceRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public InvoiceServiceRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<InvoiceServiceGetRequest>?> GetAll(int invoiceId, bool? plain = false)
        {
            var invoiceServices = _dbContext.InvoiceService;
            if (plain == false)
            {
                invoiceServices
                .Include(i => i.InvoiceItem)
                .ThenInclude(i => i!.Tariff);
            }
            var invoiceServicesList = await invoiceServices
                .Select(i => new InvoiceServiceGetRequest(i, plain))
                .Where(i => i.InvoiceId == invoiceId)
                .ToListAsync();
            return invoiceServicesList;
        }
        public async Task<InvoiceServiceGetRequest?> GetById(int itemServiceId, bool? plain = false)
        {
            var invoiceService = _dbContext.InvoiceService;
            if (plain == false)
            {
                invoiceService
                    .Include(i => i.InvoiceItem)
                    .ThenInclude(i => i!.Tariff);
            }
            var invoiceServiceCall = await invoiceService.FindAsync(itemServiceId);
            var invoiceServiceResult = new InvoiceServiceGetRequest(invoiceServiceCall, plain);
            return invoiceServiceCall is not null ? invoiceServiceResult : null;
        }
        public async Task<int?> Add(int InvoiceId, InvoiceServiceExtendedAddRequest invoiceService)
        {
            var newInvoiceService = new InvoiceService
            {
                InvoiceId = InvoiceId, 
                InvoiceItemId = invoiceService.ItemId,
                Units = invoiceService.Units,
                PricePerUnit = invoiceService.PricePerUnit,
                BasePrice = invoiceService.BasePrice,
                VAT = invoiceService.VAT,
                Total = invoiceService.Total,
            };
            var entity = await _dbContext.InvoiceService.AddAsync(newInvoiceService);

            return entity.State == EntityState.Added ? entity.Entity.Id : null;
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

            localInvoiceservice.Units = invoiceService.Units ?? localInvoiceservice.Units;
            localInvoiceservice.PricePerUnit = invoiceService.PricePerUnit ?? localInvoiceservice.PricePerUnit;
            localInvoiceservice.PricePerUnit = invoiceService.PricePerUnit ?? localInvoiceservice.PricePerUnit;
            localInvoiceservice.PricePerUnit = invoiceService.PricePerUnit ?? localInvoiceservice.PricePerUnit;
            
            var update = _dbContext.Update(localInvoiceservice);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> Delete(int id)
        {
            var invoiceService = await Get(id);
            if (invoiceService is null) throw new DatabaseCallError("Invoice service is not in database.");

            var entity = _dbContext.InvoiceService.Remove(invoiceService);
            return entity.State == EntityState.Deleted;
        }
        public async Task<InvoiceService?> Get(int invoiceServiceId)
        {
            return await _dbContext.InvoiceService.FindAsync(invoiceServiceId);
        }
        public async Task<List<InvoiceService>?> GetByCondition(Expression<Func<InvoiceService,bool>> condition)
        {
            var result = await _dbContext.InvoiceService.Where(condition).ToListAsync();
            return result;
        }
    }
}