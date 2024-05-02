using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using Xunit;

namespace Repository
{ 
    [Collection("Sequential")]
    public class DeleteInvoice: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteInvoice()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                await db.InitInvoiceCopies();

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

                var addInvoice = await db._repository.Invoice.Add(1,invoice);

                //ASSERT
                Assert.NotNull(addInvoice);
                Assert.IsType<int>(addInvoice);

                var deleteResult = await db._repository.Invoice.Delete((int)addInvoice);
                await db._repository.Save();

                Assert.True(deleteResult);

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorWhenDeleteNonExistentInvoice()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                

                //ASSERT
                try
                {
                    var deleteResult = await db._repository.Invoice.Delete(100);
                    await db._repository.Save();
                }
                catch (Exception error)
                {
                    Assert.IsType<DatabaseCallError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}