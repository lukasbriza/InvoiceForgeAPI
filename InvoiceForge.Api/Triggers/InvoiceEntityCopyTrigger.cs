using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class InvoiceEntityCopyUpdateTrigger: IBeforeSaveTrigger<InvoiceEntityCopy>
    {
        readonly InvoiceForgeDatabaseContext _context;
        public InvoiceEntityCopyUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public async Task BeforeSave(ITriggerContext<InvoiceEntityCopy> ctx, CancellationToken cancellationToken)
        {
            if (
                ctx.ChangeType == ChangeType.Modified && 
                ctx.Entity.Outdated == true &&
                ctx.UnmodifiedEntity?.Outdated == false
                )
            {
                InvoiceEntityCopy entity = ctx.Entity;
                if (entity.OriginContractorId == null && entity.OriginClientId != null)
                {
                    List<Invoice> invoices = await _context.Invoice
                        .IgnoreAutoIncludes()
                        .Where(i => i.ClientCopyId == entity.OriginClientId && i.Outdated == false)
                        .ToListAsync();
                    invoices.ConvertAll(i => {
                        i.Outdated = true;
                        return i;
                    });
                }
                if (entity.OriginClientId == null && entity.OriginContractorId != null)
                {
                    List<Invoice> invoices = await _context.Invoice
                        .IgnoreAutoIncludes()
                        .Where(i => i.ContractorCopyId == entity.OriginContractorId && i.Outdated == false)
                        .ToListAsync();
                    invoices.ConvertAll(i => {
                        i.Outdated = true;
                        return i;
                    });
                }
            }
        }
    }
}