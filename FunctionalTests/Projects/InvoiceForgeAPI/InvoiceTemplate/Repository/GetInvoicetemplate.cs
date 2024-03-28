using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InvoiceTemplateRepository
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
                db.InitializeDbForTest();

                var userIds = await db._context.User.Select(u => u.Id).ToListAsync();
                
                //ASSERT
                userIds.ForEach(async userId => {
                    var invoiceTemplates = new InvoiceTemplateSeed().Populate().FindAll(t => t.Owner == userId);
                    var dbInvoiceTemplates = await db._repository.InvoiceTemplate.GetAll(userId);

                    Assert.NotNull(dbInvoiceTemplates);
                    Assert.IsType<List<InvoiceTemplateGetRequest>>(dbInvoiceTemplates);
                    Assert.Equal(dbInvoiceTemplates?.Count, invoiceTemplates.Count);
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
                db.InitializeDbForTest();

                var invoiceTemplatesIds = await db._context.InvoiceTemplate.Select(t => t.Id).ToListAsync();

                //ASSERT
                invoiceTemplatesIds.ForEach(async tempateId => {
                    var dbInvoiceTemplate = await db._repository.InvoiceTemplate.GetById(tempateId, true);
                    Assert.NotNull(dbInvoiceTemplate);
                    Assert.IsType<InvoiceTemplateGetRequest>(dbInvoiceTemplate);
                });

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}