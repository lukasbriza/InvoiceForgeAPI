using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InvoiceItemRepository
{
    [Collection("Sequential")]
    public class DeleteInvoiceItem: WebApplicationFactory
    {
        [Fact]
        public Task CanRemoveInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var invoiceItemIds = await db._context.InvoiceItem.Select(i => i.Id).ToListAsync();

                //ASSERT
                invoiceItemIds.ForEach(async invoiceItemId => {
                    var invoiceItemRemove = await db._repository.InvoiceItem.Delete(invoiceItemId);
                    Assert.True(invoiceItemRemove);
                });
                await db._repository.Save();
                
                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorThanDeleteNonexistentInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                try
                {
                    var removeResult = await db._repository.InvoiceItem.Delete(100);
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