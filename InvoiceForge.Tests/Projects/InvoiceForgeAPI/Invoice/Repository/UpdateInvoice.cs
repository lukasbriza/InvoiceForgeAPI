using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class UpdateInvoice: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateInvoiceById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();

                var invoice = new InvoiceAddRequestRepository {
                    TemplateId = 1,
                    NumberingId = 1,
                    InvoiceNumber = "num",
                    OrderNumber = 1,
                    BasePriceTotal = 10,
                    VATTotal = 2,
                    TotalAll = 12,
                    Currency = " CZK",

                    ClientCopyId = 1,
                    ContractorCopyId = 2,
                    UserAccountCopyId = 1,

                    Maturity = DateTime.Now,
                    Exposure = DateTime.Now,
                    TaxableTransaction = DateTime.Now,
                    Created = DateTime.Now
                };

                var addInvoice = await db._repository.Invoice.Add(1, invoice);

                //ASSERT
                Assert.NotNull(addInvoice);
                Assert.IsType<int>(addInvoice);

                var updateInvoice = new InvoiceUpdateRequest{
                    Owner = 1,
                    Maturity = new DateTime(1,1,2020),
                    Exposure = new DateTime(1,1,2020),
                    TaxableTransaction = new DateTime(1,1,2020)
                };

                var updateResult = await db._repository.Invoice.Update((int)addInvoice, updateInvoice);
                await db._repository.Save();

                var updatedInvoice = await db._context.Invoice.FindAsync(addInvoice);

                Assert.NotNull(updatedInvoice);
                Assert.IsType<Invoice>(updatedInvoice);
                Assert.True(updateResult);
                Assert.Equal(updateInvoice.Maturity, updatedInvoice?.Maturity);
                Assert.Equal(updateInvoice.Exposure, updatedInvoice?.Exposure);

                //CLEAR
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnUpdateNonExistentInvoice()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                

                //ASSERT
                try
                {
                    var updateResult = await db._repository.Invoice.Update(100, new InvoiceUpdateRequest {Maturity = DateTime.Now});
                    await db._repository.Save();
                }
                catch (Exception error)
                {
                    Assert.IsType<DatabaseCallError>(error);    
                }

                //CLIENT
                db.Dispose();
            });
        }
    
        [Fact]
        public Task ThrowErrorWhenUpdateIdenticValues()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var invoice = await db._context.Invoice.FindAsync(1);

                //ASSERT
                Assert.NotNull(invoice);
                var updateInvoice = new InvoiceUpdateRequest
                {
                    Owner = invoice!.Owner,
                    Exposure = invoice.Exposure,
                    Maturity = invoice.Maturity,
                    TaxableTransaction = invoice.TaxableTransaction
                };

                try
                {
                    var result = await db._repository.Invoice.Update(1, updateInvoice);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}