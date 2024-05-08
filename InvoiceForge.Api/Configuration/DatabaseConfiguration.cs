using InvoiceForgeApi.Triggers;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void Configure(DbContextOptionsBuilder options, string connectionString)
        {
            options.UseSqlServer(connectionString);
            options.UseTriggers(triggerOptions => {
                triggerOptions.AddTrigger<AddressUpdateTrigger>();
                triggerOptions.AddTrigger<InvoiceAddressCopyUpdateTrigger>();
                triggerOptions.AddTrigger<InvoiceEntityCopyUpdateTrigger>();
                triggerOptions.AddTrigger<ClientUpdateTrigger>();
                triggerOptions.AddTrigger<ContractorUpdateTrigger>();
                triggerOptions.AddTrigger<InvoiceTemplateUpdateTrigger>();
                triggerOptions.AddTrigger<UserAccountUpdateTrigger>();
                triggerOptions.AddTrigger<InvoiceUserAccountCopyUpdateTrigger>();
                triggerOptions.AddTrigger<TrackableTrigger>();
            });
        }
    }
}