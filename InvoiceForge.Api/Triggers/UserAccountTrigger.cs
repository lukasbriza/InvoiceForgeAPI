using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Triggers
{
    public class UserAccountUpdateTrigger: IBeforeSaveTrigger<UserAccount>
    {
        readonly InvoiceForgeDatabaseContext _context;

        public UserAccountUpdateTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public async Task BeforeSave(ITriggerContext<UserAccount> ctx, CancellationToken cancellationToken)
        {
            if (ctx.ChangeType == ChangeType.Modified)
            {
                UserAccount entity = ctx.Entity;
                List<InvoiceUserAccountCopy> linkedUserAccount = await _context.InvoiceUserAccountCopy
                    .IgnoreAutoIncludes()
                    .Where(a => a.OriginId == entity.Id && a.Outdated == false)
                    .ToListAsync();
                linkedUserAccount.ConvertAll(a => {
                    a.Outdated = true;
                    return a;
                });
            }
        }

    }
}