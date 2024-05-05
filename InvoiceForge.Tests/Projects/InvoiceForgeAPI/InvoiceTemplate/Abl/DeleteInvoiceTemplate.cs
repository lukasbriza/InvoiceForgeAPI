using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Abl.invoiceTemplate;
using InvoiceForgeApi.Errors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class DeleteInvoiceTemplate: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteInvoiceTemplate()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteInvoiceTemplateAbl(db._repository);
                
                var templateIds = await db._context.InvoiceTemplate
                    .Where(t => t.Invoices!.Count == 0)
                    .Select(t => t.Id)
                    .ToListAsync();
                
                //ASSERT
                async Task call(int id)
                {
                    var result = await abl.Resolve(id);
                    Assert.True(result);
                    var control = await db._context.InvoiceTemplate.FindAsync(id);
                    Assert.Null(control);
                };

                Assert.NotNull(templateIds);

                templateIds?.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDeleteTemplateWithReference()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteInvoiceTemplateAbl(db._repository);

                var templateIds = await db._context.InvoiceTemplate
                    .Where(t => t.Invoices!.Count > 0)
                    .Select(t => t.Id)
                    .ToListAsync();

                //ASSERT
                async Task call(int id)
                {
                    try
                    {
                        var result = await abl.Resolve(id);
                    }
                    catch (Exception ex)
                    {
                        Assert.IsType<NoPossessionError>(ex);
                    }
                };

                Assert.NotNull(templateIds);

                templateIds?.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}