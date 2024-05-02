using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class ContractorUpdateTrigger: IBeforeSaveTrigger<Contractor>
    {
        readonly InvoiceForgeDatabaseContext _context;

        public ContractorUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

         public async Task BeforeSave(ITriggerContext<Contractor> ctx, CancellationToken cancellationToken)
         {
            if (ctx.ChangeType == ChangeType.Modified)
            {
                Contractor entity = ctx.Entity;
                List<InvoiceEntityCopy> linkedContractorEntity = await _context.InvoiceEntityCopy
                    .IgnoreAutoIncludes()
                    .Where(e => e.OriginContractorId == entity.Id && e.OriginClientId == null && e.Outdated == false)
                    .ToListAsync();
                linkedContractorEntity.ConvertAll(e => {
                    e.Outdated = true;
                    return e;
                });
            }
         }
    }
}