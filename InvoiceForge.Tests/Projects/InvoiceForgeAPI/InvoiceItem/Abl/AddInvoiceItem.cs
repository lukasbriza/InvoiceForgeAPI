using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Abl.invoiceItem;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class AddInvoiceItem: WebApplicationFactory
    {
        [Fact]
        public Task CannAddInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddInvoiceItemAbl(db._repository);

                var invoiceItem = new InvoiceItemAddRequest
                {
                    ItemName = "TestItem",
                    TariffId = 1
                };

                //ASSERT
                var result = await abl.Resolve(1, invoiceItem);
                Assert.True(result);

                var control = await db._context.InvoiceItem.Where(i => 
                    i.ItemName == invoiceItem.ItemName &&
                    i.TariffId == invoiceItem.TariffId
                ).ToListAsync();

                Assert.NotNull(control);
                if (control is not null)
                {
                    Assert.Single(control);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidTariff()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddInvoiceItemAbl(db._repository);

                var invoiceItem = new InvoiceItemAddRequest
                {
                    ItemName = "TestItem",
                    TariffId = 100
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, invoiceItem);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NoEntityError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDuplicitInvoiceItemName()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddInvoiceItemAbl(db._repository);

                var invoiceItem = new InvoiceItemAddRequest
                {
                    ItemName = "TestItem",
                    TariffId = 1
                };

                await db._repository.InvoiceItem.Add(1, invoiceItem);
                await db._context.SaveChangesAsync();

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, invoiceItem);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NotUniqueEntityError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}