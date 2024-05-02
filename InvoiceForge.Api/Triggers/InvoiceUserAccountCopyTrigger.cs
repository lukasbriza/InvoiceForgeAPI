using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class InvoiceUserAccountCopyUpdateTrigger: IBeforeSaveTrigger<InvoiceUserAccountCopy>
    {
        readonly InvoiceForgeDatabaseContext _context;
        public InvoiceUserAccountCopyUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public async Task BeforeSave(ITriggerContext<InvoiceUserAccountCopy> ctx, CancellationToken cancellationToken)
        {
            if (
                ctx.ChangeType == ChangeType.Modified &&
                ctx.Entity.Outdated == true &&
                ctx.UnmodifiedEntity?.Outdated == false
                )
            {
                InvoiceUserAccountCopy entity = ctx.Entity;
                List<Invoice> invoices = await _context.Invoice
                    .IgnoreAutoIncludes()
                    .Where(i => i.UserAccountCopyId == entity.Id && i.Outdated == false)
                    .ToListAsync();
                invoices.ConvertAll(i => {
                    i.Outdated = true;
                    return i;
                });
            }
        }
    }
}