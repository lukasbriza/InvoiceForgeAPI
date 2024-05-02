using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class InvoiceAddressCopyUpdateTrigger: IBeforeSaveTrigger<InvoiceAddressCopy>
    {
        readonly InvoiceForgeDatabaseContext _context;

        public InvoiceAddressCopyUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public async Task BeforeSave(ITriggerContext<InvoiceAddressCopy> ctx, CancellationToken cancellationToken)
        {
            if (
                ctx.ChangeType == ChangeType.Modified && 
                ctx.Entity.Outdated == true &&
                ctx.UnmodifiedEntity?.Outdated == false
                )
            {
                InvoiceAddressCopy entity = ctx.Entity;
                List<InvoiceEntityCopy> entityCopies = await _context.InvoiceEntityCopy
                    .IgnoreAutoIncludes()
                    .Where(e => e.AddressCopyId == entity.Id && e.Outdated == false)
                    .ToListAsync();
                entityCopies.ConvertAll(e => {
                    e.Outdated = true;
                    return e;
                });
            }
        }
    }
}