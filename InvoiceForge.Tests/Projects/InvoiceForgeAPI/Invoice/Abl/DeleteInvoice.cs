using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Abl.invoice;

using InvoiceForgeApi.Errors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class DeleteInvoice: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteInvoice()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var abl = new DeleteInvoiceAbl(db._repository);

                var invoices = await db._context.Invoice.ToListAsync();

                //ASSERT
                async Task call(int invoiceId)
                {
                    var result = await abl.Resolve(invoiceId);
                    Assert.True(result);
                };

                invoices?.ForEach(invoice => {
                    var task = call(invoice.Id);
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnNonExistentInvoiceDelete()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var abl = new DeleteInvoiceAbl(db._repository);

                //ASSERT
                try
                {
                    var result = await abl.Resolve(100);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NoEntityError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}