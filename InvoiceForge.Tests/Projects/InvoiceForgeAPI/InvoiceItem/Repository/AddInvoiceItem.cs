using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace InvoiceItemRepository
{
    [Collection("Sequential")]
    public class AddInvoiceItem: WebApplicationFactory
    {
        [Fact]
        public Task CanAddInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                var invoiceItem = new InvoiceItemAddRequest
                {
                    ItemName = "TestName",
                    TariffId = 1
                };

                var addInvoiceItem = await db._repository.InvoiceItem.Add(1, invoiceItem);

                Assert.NotNull(addInvoiceItem);
                Assert.IsType<int>(addInvoiceItem);

                var addedInvoiceItem = await db._context.InvoiceItem.FindAsync((int)addInvoiceItem);

                Assert.Equal(invoiceItem.ItemName, addedInvoiceItem?.ItemName);
                Assert.Equal(invoiceItem.TariffId, addedInvoiceItem?.TariffId);
                
                //CLEAN
                db.Dispose();
            });
        }
    }
}