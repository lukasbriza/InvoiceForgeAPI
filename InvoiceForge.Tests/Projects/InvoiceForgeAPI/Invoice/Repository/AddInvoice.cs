using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
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
                await db.InitInvoiceCopies();
                
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

                    ClientCopyId = 1,
                    ContractorCopyId = 2,
                    UserAccountCopyId = 1,
                    
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