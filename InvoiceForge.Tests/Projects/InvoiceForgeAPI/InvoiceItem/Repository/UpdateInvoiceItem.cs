using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using Xunit;

namespace InvoiceItemRepository
{
    [Collection("Sequential")]
    public class UpdateInvoiceItem: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateInvoiceItemById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                var updateItem = new InvoiceItemUpdateRequest { Owner = 1, ItemName = "TestItemName" };
                var updateItemResult = await db._repository.InvoiceItem.Update(1, updateItem);
                await db._repository.Save();

                Assert.True(updateItemResult);

                var updatedItem = await db._context.InvoiceItem.FindAsync(1);
                
                Assert.NotNull(updatedItem);
                Assert.IsType<InvoiceItem>(updatedItem);
                Assert.Equal(updatedItem?.ItemName, updateItem.ItemName);
                Assert.Equal(updatedItem?.Owner, updateItem.Owner);

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnUpdateNonExistentInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                try
                {
                    var updateItem = new InvoiceItemUpdateRequest { Owner = 1, ItemName = "TestItemName" };
                    var updateResult = await db._repository.InvoiceItem.Update(100, updateItem);
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