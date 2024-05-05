using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    [Collection("Sequential")]
    public class GetInvoice: WebApplicationFactory
    {
        [Fact]
        public Task ReturnInvoicesForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                await db.InitInvoiceCopies();
                
                //ASSERT
                var invoices = await db._context.Invoice.ToListAsync();
                var repositoryInvoices = await db._repository.Invoice.GetAll(1, true);

                Assert.NotNull(repositoryInvoices);
                Assert.IsType<List<InvoiceGetRequest>>(repositoryInvoices);
                Assert.Equal(invoices.Count, repositoryInvoices?.Count);

                //CLEAN
                db.Dispose();
            });
        }
    }
}