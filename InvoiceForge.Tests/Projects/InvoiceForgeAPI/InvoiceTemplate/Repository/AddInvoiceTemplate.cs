using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class AddInvoiceTemplate: WebApplicationFactory
    {
        [Fact]
        public Task CanAddInvoiceTemplate()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                //ASSERT
                var addInvoiceTemplate = new InvoiceTemplateAddRequest
                {
                    ClientId = 1,
                    ContractorId = 1,
                    CurrencyId = 1,
                    UserAccountId = 1,
                    TemplateName = "TemplateTestName",
                    NumberingId = 1
                };

                var addInvoiceTemplateResult = await db._repository.InvoiceTemplate.Add(1, addInvoiceTemplate);
                Assert.NotNull(addInvoiceTemplateResult);

                if (addInvoiceTemplateResult is not null)
                {
                    Assert.IsType<int>(addInvoiceTemplateResult);
                    var newInvoiceTemplate = await db._context.InvoiceTemplate.FindAsync(addInvoiceTemplateResult);

                    Assert.Equal(addInvoiceTemplate.ClientId, newInvoiceTemplate?.ClientId);
                    Assert.Equal(addInvoiceTemplate.ContractorId, newInvoiceTemplate?.ContractorId);
                    Assert.Equal(addInvoiceTemplate.UserAccountId, newInvoiceTemplate?.UserAccountId);
                    Assert.Equal(addInvoiceTemplate.TemplateName, newInvoiceTemplate?.TemplateName);
                    Assert.Equal(addInvoiceTemplate.NumberingId, newInvoiceTemplate?.NumberingId);
                }

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}