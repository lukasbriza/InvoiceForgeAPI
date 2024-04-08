using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;
using Xunit;

namespace InvoiceRepository
{
    [Collection("Sequential")]
    public class GetInvoice: WebApplicationFactory
    {
        [Fact]
        public Task ReturnInvoicesForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var dbclient = await db._context.Client.FindAsync(1);
                var contractor = await db._context.Contractor.FindAsync(1);
                var userAccount = await db._context.UserAccount.FindAsync(1);
                
                //ASSERT
                var invoices = new List<Invoice>();

                for (var i = 0; i < 5; i++)
                {
                    invoices.Add(
                        new Invoice {
                            Outdated = false,
                            Owner = 1,
                            TemplateId = 1,
                            NumberingId = 1,
                            InvoiceNumber = "num",
                            OrderNumber = i + 1,
                            BasePriceTotal = 10,
                            VATTotal = 2,
                            TotalAll = 12,
                            Currency = " CZK",
                            ClientLocal = new ClientGetRequest(dbclient, true),
                            ContractorLocal = new ContractorGetRequest(contractor, true),
                            UserAccountLocal = new UserAccountGetRequest(userAccount, true),
                            Maturity = DateTime.Now,
                            Exposure = DateTime.Now,
                            TaxableTransaction = DateTime.Now,
                            Created = DateTime.Now
                        }
                    );
                }
               
                await db._context.Invoice.AddRangeAsync(invoices);
                await db._context.SaveChangesAsync();

                var repositoryInvoices = await db._repository.Invoice.GetAll(1, true);

                Assert.NotNull(repositoryInvoices);
                Assert.IsType<List<InvoiceGetRequest>>(repositoryInvoices);
                Assert.Equal(invoices.Count, repositoryInvoices?.Count);

                //CLEAN
                db.Dispose();
            });
        }
    }
}