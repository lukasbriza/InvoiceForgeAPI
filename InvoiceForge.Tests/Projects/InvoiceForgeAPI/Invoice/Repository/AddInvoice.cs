using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using Xunit;

namespace InvoiceRepository
{
    [Collection("Sequential")]
    public class AddInvoice: WebApplicationFactory
    {
        [Fact]
        public Task CanAddInvoice()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                var dbclient = await db._context.Client.FindAsync(1);
                var contractor = await db._context.Contractor.FindAsync(1);
                var userAccount = await db._context.UserAccount.FindAsync(1);
                
                //ASSERT
                var invoice = new InvoiceAddRequestRepository {
                    TemplateId = 1,
                    NumberingId = 1,
                    InvoiceNumber = "num",
                    OrderNumber = 1,
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
                };

                var addResult = await db._repository.Invoice.Add(1, invoice);
                Assert.NotNull(addResult);
                Assert.IsType<int>(addResult);

                var addedInvoice = await db._context.Invoice.FindAsync(addResult);
                Assert.NotNull(addedInvoice);
                Assert.IsType<Invoice>(addedInvoice);

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}