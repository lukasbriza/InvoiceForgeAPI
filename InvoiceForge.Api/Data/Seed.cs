using InvoiceForgeApi.Data.SeedClasses;

namespace InvoiceForgeApi.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<InvoiceForgeDatabaseContext>();
                PopulateDatabase(context);
            }
        }
        public static void PopulateDatabase(InvoiceForgeDatabaseContext? context)
        {
                context!.Database.EnsureCreated();
                //Bank
                if(!context.Bank.Any())
                {
                    context.Bank.AddRange(new BankSeed().Populate());
                    context.SaveChanges();
                }
                //Country
                if(!context.Country.Any())
                {
                    context.Country.AddRange(new CountrySeed().Populate());
                    context.SaveChanges();
                }
                //Currency
                if(!context.Currency.Any())
                {
                    context.Currency.AddRange(new CurrencySeed().Populate());
                    context.SaveChanges();
                }
                //Tariff
                if(!context.Tariff.Any())
                {
                    context.Tariff.AddRange(new TariffSeed().Populate());
                    context.SaveChanges();
                }
            context.SaveChanges();
        }
        
    }
}
