using EntityFrameworkCore.Triggered;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Triggers
{
    public class TrackableTrigger: IBeforeSaveTrigger<ITrackable>
    {
        readonly InvoiceForgeDatabaseContext _context;

        public TrackableTrigger(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }

        public Task BeforeSave(ITriggerContext<ITrackable> ctx, CancellationToken cancellationToken)
        {
            if (ctx.ChangeType == ChangeType.Modified)
            {
                ctx.Entity.Updated = DateTime.UtcNow;
            }
            if (ctx.ChangeType == ChangeType.Added)
            {
                ctx.Entity.Created = DateTime.UtcNow;
            }

            return Task.CompletedTask;
        }
    }
}