using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class AddressUpdateTrigger: IBeforeSaveTrigger<Address>
    {
        readonly InvoiceForgeDatabaseContext _context;

        public AddressUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public async Task BeforeSave(ITriggerContext<Address> ctx, CancellationToken cancellationToken)
        {
            if (ctx.ChangeType == ChangeType.Modified)
            {   
                Address entity = ctx.Entity;
                List<InvoiceAddressCopy>? linkedAddressCopy = await _context.InvoiceAddressCopy
                    .IgnoreAutoIncludes()
                    .Where(a => a.OriginId == entity.Id && a.Outdated == false)
                    .ToListAsync();
                linkedAddressCopy.ConvertAll(a => {
                    a.Outdated = true;
                    return a;
                });
            }
        }
    }
}
