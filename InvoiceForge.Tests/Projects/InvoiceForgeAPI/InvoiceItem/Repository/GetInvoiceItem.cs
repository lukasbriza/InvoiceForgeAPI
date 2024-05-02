using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class GetInvoiceItem: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfInvoiceItemsForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var userIds = await db._context.User.Select(u => u.Id).ToListAsync();

                //ASSERT
                async Task call(int userId){
                    var invoiceItems = new InvoiceItemSeed().Populate().FindAll(i => i.Owner == userId);
                    var dbInvoiceItems = await db._repository.InvoiceItem.GetAll(userId, true);

                    Assert.NotNull(dbInvoiceItems);
                    Assert.Equal(invoiceItems.Count, dbInvoiceItems?.Count);
                }

                userIds.ForEach(userId => {
                    var task = call(userId);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task CanGetInvoiceItemsById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var invoiceItemIds = await db._context.InvoiceItem.Select(u => u.Id).ToListAsync();

                //ASSERT
                async Task call(int invoiceItemId){
                    var invoiceItemResult = await db._repository.InvoiceItem.GetById(invoiceItemId, true);
                    Assert.NotNull(invoiceItemResult);
                    Assert.IsType<InvoiceItemGetRequest>(invoiceItemResult);
                }

                invoiceItemIds.ForEach(invoiceItemId => {
                    var task = call(invoiceItemId);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

    }
}