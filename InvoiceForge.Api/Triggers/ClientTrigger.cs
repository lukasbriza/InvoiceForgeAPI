using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class ClientUpdateTrigger: IBeforeSaveTrigger<Client>
    {
        readonly InvoiceForgeDatabaseContext _context;

        public ClientUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public async Task BeforeSave(ITriggerContext<Client> ctx, CancellationToken cancellationToken)
        {
            if (ctx.ChangeType == ChangeType.Modified)
            {
                Client entity = ctx.Entity;
                List<InvoiceEntityCopy> linkedClientEntities = await _context.InvoiceEntityCopy
                    .IgnoreAutoIncludes()
                    .Where(e => e.OriginClientId == entity.Id && e.OriginContractorId == null && e.Outdated == false)
                    .ToListAsync();
                linkedClientEntities.ConvertAll(e => {
                    e.Outdated = true;
                    return e;
                });
            }
        }
    }
}