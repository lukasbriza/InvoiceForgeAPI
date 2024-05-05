using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Abl.invoice;

using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class UpdateInvoice: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateInvoice()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var abl = new UpdateInvoiceAbl(db._repository);

                var invoice = await db._context.Invoice.FindAsync(1);
                //ASSERT
                Assert.NotNull(invoice);
                var updateInvoice = new InvoiceUpdateRequest
                {
                    Owner = invoice!.Owner,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1)
                };

                var resolve = await abl.Resolve(invoice.Id, updateInvoice);
                Assert.True(resolve);
                
                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnIvalidUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var abl = new UpdateInvoiceAbl(db._repository);

                //ASSERT
                var invoiceUpdate = new InvoiceUpdateRequest
                {
                    Owner = 100,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1)
                };

                try
                {
                    var result = await abl.Resolve(1,invoiceUpdate);
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
        public Task ThrowErrorOnInvalidInvoiceId()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var abl = new UpdateInvoiceAbl(db._repository);
                
                //ASSERT
                var invoiceUpdate = new InvoiceUpdateRequest
                {
                    Owner = 1,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1)
                };
                try
                {
                    var result = await abl.Resolve(100, invoiceUpdate);    
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
        public Task ThrowErrorOnInvoiceIdOutOfPossession()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var abl = new UpdateInvoiceAbl(db._repository);

                var invoice = await db._context.Invoice.FindAsync(1);
                invoice!.Owner = 2;
                
                db._context.Invoice.Update(invoice);
                await db._context.SaveChangesAsync();

                var invoiceUpdate = new InvoiceUpdateRequest
                {
                    Owner = 1,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1)
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, invoiceUpdate);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NoPossessionError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}