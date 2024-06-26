using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class GetInvoicetemplate: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfInvoiceTemplatesForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var userIds = await db._context.User.Select(u => u.Id).ToListAsync();
                
                //ASSERT
                async Task call(int userId){
                    var invoiceTemplates = new InvoiceTemplateSeed().Populate().FindAll(t => t.Owner == userId);
                    var dbInvoiceTemplates = await db._repository.InvoiceTemplate.GetAll(userId);

                    Assert.NotNull(dbInvoiceTemplates);
                    Assert.IsType<List<InvoiceTemplateGetRequest>>(dbInvoiceTemplates);
                    Assert.Equal(dbInvoiceTemplates?.Count, invoiceTemplates.Count);
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
        public Task CanGetInvoiceTemplateById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                

                var invoiceTemplatesIds = await db._context.InvoiceTemplate.Select(t => t.Id).ToListAsync();

                //ASSERT
                async Task call(int templateId){
                    var dbInvoiceTemplate = await db._repository.InvoiceTemplate.GetById(templateId, true);
                    Assert.NotNull(dbInvoiceTemplate);
                    Assert.IsType<InvoiceTemplateGetRequest>(dbInvoiceTemplate);
                }

                invoiceTemplatesIds.ForEach(templateId => {
                    var task = call(templateId);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}