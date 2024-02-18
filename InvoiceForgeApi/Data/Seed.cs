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

                context.Database.EnsureCreated();
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
                //User
                if (!context.User.Any())
                {
                    context.User.AddRange(new UserSeed().Populate());
                    context.SaveChanges();
                }
                //InvoiceTemplate
                if (!context.InvoiceTemplate.Any()) 
                {
                    context.InvoiceTemplate.AddRange(new InvoiceTemplateSeed().Populate());
                    context.SaveChanges();
                }
                //Address
                if (!context.Address.Any())
                {
                    context.Address.AddRange(new AddressSeed().Populate());
                    context.SaveChanges();
                }
                //Client
                if(!context.Client.Any()) 
                {
                    context.Client.AddRange(new ClientSeed().Populate());
                    context.SaveChanges();
                }
                //Contractor
                if (!context.Contractor.Any())
                {
                    context.Contractor.AddRange(new ContractorSeed().Populate());
                    context.SaveChanges();
                }
                //UserAccount
                if (!context.UserAccount.Any())
                {
                    context.UserAccount.AddRange(new UserAccountSeed().Populate());
                    context.SaveChanges();
                }
            }
        }
    }
}
