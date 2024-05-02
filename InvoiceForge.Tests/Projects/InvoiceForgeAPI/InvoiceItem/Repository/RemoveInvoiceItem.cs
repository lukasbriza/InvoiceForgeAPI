using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
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
                
                var invoiceItemIds = await db._context.InvoiceItem.Select(i => i.Id).ToListAsync();

                //ASSERT
                async Task call(int invoiceItemId){
                    var invoiceItemRemove = await db._repository.InvoiceItem.Delete(invoiceItemId);
                    Assert.True(invoiceItemRemove);
                }
                invoiceItemIds.ForEach(invoiceItemId => {
                    var task = call(invoiceItemId);
                    task.Wait();
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

                //ASSERT
                try
                {
                    var removeResult = await db._repository.InvoiceItem.Delete(100);
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