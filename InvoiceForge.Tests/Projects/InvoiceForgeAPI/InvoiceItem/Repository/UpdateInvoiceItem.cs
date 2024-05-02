using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
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
                

                //ASSERT
                var updateItem = new InvoiceItemUpdateRequest { Owner = 1, ItemName = "TestItemName", TariffId = 1 };
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

        [Fact]
        public Task ThrowErrorWhenUpdateIdenticValues()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var invoiceItem = await db._context.InvoiceItem.FindAsync(1);

                //ASSERT
                var updateInvoiceItem = new InvoiceItemUpdateRequest
                {
                    Owner = invoiceItem.Owner,
                    ItemName = invoiceItem.ItemName,
                    TariffId = invoiceItem.TariffId,
                };

                try
                {
                    var result = await db._repository.InvoiceItem.Update(1, updateInvoiceItem);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}