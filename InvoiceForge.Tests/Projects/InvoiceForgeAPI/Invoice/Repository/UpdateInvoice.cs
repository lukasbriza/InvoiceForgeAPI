using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models;
using Xunit;

namespace InvoiceRepository
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
                db.InitializeDbForTest();
                var dbclient = await db._context.Client.FindAsync(1);
                var contractor = await db._context.Contractor.FindAsync(1);
                var userAccount = await db._context.UserAccount.FindAsync(1);

                var invoice = new InvoiceAddRequestRepository {
                    TemplateId = 1,
                    NumberingId = 1,
                    InvoiceNumber = "num",
                    OrderNumber = 1,
                    BasePriceTotal = 10,
                    VATTotal = 2,
                    TotalAll = 12,
                    Currency = " CZK",
                    ClientLocal = new ClientGetRequest(dbclient, true),
                    ContractorLocal = new ContractorGetRequest(contractor, true),
                    UserAccountLocal = new UserAccountGetRequest(userAccount, true),
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
                    Maturity = DateTime.MaxValue,
                    Exposure = DateTime.MinValue,
                    TaxableTransaction = DateTime.Now
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
                db.InitializeDbForTest();

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
    }
}