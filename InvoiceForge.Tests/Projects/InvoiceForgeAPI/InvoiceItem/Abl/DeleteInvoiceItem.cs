using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Abl.invoiceItem;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class DeleteInvoiceItem: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteInvoiceItemAbl(db._repository);

                var ids = await db._context.InvoiceItem
                    .Where(i => i.InvoiceServices!.Count == 0)
                    .Select(i => i.Id)
                    .ToListAsync();

                //ASSERT
                Assert.NotNull(ids);

                async Task call(int id)
                {
                    var result = await abl.Resolve(id);
                    Assert.True(result);
                }

                ids?.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDeleteItemWithReference()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteInvoiceItemAbl(db._repository);

                var ids = await db._context.InvoiceItem
                    .Where(i => i.InvoiceServices!.Count > 0)
                    .Select(i => i.Id)
                    .ToListAsync();

                //ASSERT
                Assert.NotNull(ids);

                async Task call(int id)
                {
                    try
                    { 
                        var result = await abl.Resolve(id);
                    }
                    catch (Exception ex)
                    {    
                        Assert.IsType<ValidationError>(ex);
                    }
                }

                ids?.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}