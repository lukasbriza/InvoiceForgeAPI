using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace InvoiceTemplateRepository
{
    [Collection("Sequential")]
    public class UpdateInvoiceTemplate: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateInvoiceTemplateById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var invoiceTempalteToCompare = await db._repository.InvoiceTemplate.GetById(1);

                //ASSERT
                Assert.NotNull(invoiceTempalteToCompare);

                if (invoiceTempalteToCompare is not null)
                {
                    var updateInvoiceTemplate = new InvoiceTemplateUpdateRequest
                    {
                        Owner = invoiceTempalteToCompare.Owner,
                        ClientId = 2,
                        ContractorId = 1,
                        UserAccountId = 1,
                        CurrencyId = 1,
                        TemplateName = "TemplateTestName",
                        NumberingId = 2
                    };

                    var updateInvoiceTemplateResult = await db._repository.InvoiceTemplate.Update(1, updateInvoiceTemplate);
                    await db._repository.Save();
                    Assert.True(updateInvoiceTemplateResult);

                    var updatedInvoiceTemplate = await db._context.InvoiceTemplate.FindAsync(1);
                    Assert.NotNull(updatedInvoiceTemplate);

                    if (updatedInvoiceTemplate is not null)
                    {
                        Assert.Equal(updateInvoiceTemplate.Owner, updatedInvoiceTemplate?.Owner);
                        Assert.Equal(updateInvoiceTemplate.ClientId, updatedInvoiceTemplate?.ClientId);
                        Assert.Equal(updateInvoiceTemplate.ContractorId, updatedInvoiceTemplate?.ContractorId);
                        Assert.Equal(updateInvoiceTemplate.UserAccountId, updatedInvoiceTemplate?.UserAccountId);
                        Assert.Equal(updateInvoiceTemplate.TemplateName, updatedInvoiceTemplate?.TemplateName);
                        Assert.Equal(updateInvoiceTemplate.NumberingId, updatedInvoiceTemplate?.NumberingId);
                    }
                }

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}