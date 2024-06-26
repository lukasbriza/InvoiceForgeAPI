using FunctionalTests.Projects.InvoiceForgeApi;

using InvoiceForgeApi.Errors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class RemoveInvoiceTemplate: WebApplicationFactory  
    {
        [Fact]
        public Task CanRemoveInvoiceTemplateById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var dbTemplateIds = await db._context.InvoiceTemplate.Select(t => t.Id).ToListAsync();

                //ASSERT
                dbTemplateIds.ForEach(async templateId => {
                    var deleteResult = await db._repository.InvoiceTemplate.Delete(templateId);
                    Assert.True(deleteResult);
                });

                await db._repository.Save();

                dbTemplateIds.ForEach(async templateId => {
                    var deletedInvoiceTemplate = await  db._context.InvoiceTemplate.FindAsync(templateId);
                    Assert.Null(deletedInvoiceTemplate);
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnRemoveNonExistentInvoiceTemplate()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                

                //ASSERT
                try
                {
                    var removeResult = await db._repository.InvoiceTemplate.Delete(100);
                    await db._repository.Save();
                }
                catch (Exception error)
                {
                    Assert.IsType<NoEntityError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}