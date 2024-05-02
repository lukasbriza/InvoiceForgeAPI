using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class InvoiceTemplateUpdateTrigger: IBeforeSaveTrigger<InvoiceTemplate>
    {
        readonly InvoiceForgeDatabaseContext _context;

        public InvoiceTemplateUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public async Task BeforeSave(ITriggerContext<InvoiceTemplate> ctx, CancellationToken cancellationToken)
        {
            if (ctx.ChangeType == ChangeType.Modified)
            {
                InvoiceTemplate entity = ctx.Entity;
                List<Invoice> invoices = await _context.Invoice
                    .Where(i => 
                        i.TemplateId == entity.Id &&
                        i.Outdated == false
                    ).ToListAsync();
                invoices?.ConvertAll(i => {
                    i.Outdated = true;
                    return i;
                });
            }
        }
    }
}