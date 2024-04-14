using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InvoiceItemRepository
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
                db.InitializeDbForTest();
                
                var userIds = await db._context.User.Select(u => u.Id).ToListAsync();

                //ASSERT
                userIds.ForEach(async userId => {
                    var invoiceItems = new InvoiceItemSeed().Populate().FindAll(i => i.Owner == userId);
                    var dbInvoiceItems = await db._repository.InvoiceItem.GetAll(userId, true);

                    Assert.NotNull(dbInvoiceItems);
                    Assert.Equal(invoiceItems.Count, dbInvoiceItems?.Count);
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
                db.InitializeDbForTest();
                
                var invoiceItemIds = await db._context.InvoiceItem.Select(u => u.Id).ToListAsync();

                //ASSERT
                invoiceItemIds.ForEach(async invoiceItemId => {
                    var invoiceItemResult = await db._repository.InvoiceItem.GetById(invoiceItemId, true);
                    Assert.NotNull(invoiceItemResult);
                    Assert.IsType<InvoiceItemGetRequest>(invoiceItemResult);
                });

                //CLEAN
                db.Dispose();
            });
        }

    }
}