using InvoiceForgeApi.Model;
using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Data
{
    public class Seed
    {
        public static void SeedCodeLists(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<InvoiceForgeDatabaseContext>();

                context.Database.EnsureCreated();

                //If no clients
                if(!context.User.Any())
                {
                    new User()
                    {
                        AuthenticationId = 789,
                    };
                }
                if(!context.Bank.Any())
                {
                    context.Bank.AddRange(new List<Bank>()
                    {
                        new Bank()
                        {
                            Value = "Komerční banka, a.s.",
                            Shortcut = "0100",
                            SWIFT = "KOMBCZPP"
                        },
                        new Bank()
                        {
                            Value = "Československá obchodní banka, a.s.",
                            Shortcut = "0300",
                            SWIFT = "CEKOCZPP"
                        },
                        new Bank()
                        {
                            Value = "MONETA Money Bank, a.s.",
                            Shortcut = "0600",
                            SWIFT = "AGBACZPP"
                        },
                        new Bank()
                        {
                            Value = "Česká spořitelna, a.s.",
                            Shortcut = "0800",
                            SWIFT = "GIBACZPX"
                        },
                        new Bank()
                        {
                            Value = "Fio banka, a.s.",
                            Shortcut = "2010",
                            SWIFT = "FIOBCZPP"
                        },
                        new Bank()
                        {
                            Value = "TRINITY BANK a.s.",
                            Shortcut = "2070",
                            SWIFT = "MPUBCZPP"
                        },
                        new Bank()
                        {
                            Value = "Air Bank a. s.",
                            Shortcut = "3030",
                            SWIFT = "AIRACZPP"
                        },
                        new Bank()
                        {
                            Value = "ING Bank N.V.",
                            Shortcut = "3500",
                            SWIFT = "INGBCZPP"
                        },
                        new Bank()
                        {
                            Value = "Raiffeisenbank, a.s.",
                            Shortcut = "5500",
                            SWIFT = "RZBCCZPP"
                        },
                        new Bank()
                        {
                            Value = "J&T BANKA, a.s.",
                            Shortcut = "5800",
                            SWIFT = "JTBPCZPP"
                        },
                        new Bank()
                        {
                            Value = "Hypoteční banka, a.s.",
                            Shortcut = "2100",
                        }
                    });
                }
                if(!context.Country.Any())
                {
                    context.Country.AddRange(new List<Country>()
                    {
                        new Country()
                        {
                            Value = "Česká republika",
                            Shortcut = "CZE"
                        }
                    });
                }
            }
        }
    }
}
