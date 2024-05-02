using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Abl.invoiceTemplate;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
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
                var abl = new AddInvoiceTemplateAbl(db._repository);

                var add = new InvoiceTemplateAddRequest
                {
                    ClientId = 1,
                    ContractorId = 1,
                    UserAccountId = 1,
                    CurrencyId = 1,
                    TemplateName = "TestTemplateAdd",
                    NumberingId = 1
                };

                //ASSERT
                var result = await abl.Resolve(1, add);
                Assert.True(result);

                var control = await db._context.InvoiceTemplate
                    .Where(t => 
                        t.TemplateName == add.TemplateName &&
                        t.ContractorId == add.ContractorId &&
                        t.UserAccountId == add.UserAccountId &&
                        t.CurrencyId == add.CurrencyId &&
                        t.NumberingId == add.NumberingId
                    ).ToListAsync();
                
                Assert.NotNull(control);
                if (control is not null)
                {
                    Assert.Equal(control[0].TemplateName, add.TemplateName);
                    Assert.Equal(control[0].CurrencyId, add.CurrencyId);
                    Assert.Equal(control[0].ClientId, add.ClientId);
                    Assert.Equal(control[0].UserAccountId, add.UserAccountId);
                    Assert.Equal(control[0].NumberingId, add.NumberingId);
                };

                //CLEAN
                db.Dispose();
            });
        }
        
        [Fact]
        public Task ThrowErrorOnInvalidClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddInvoiceTemplateAbl(db._repository);

                var add = new InvoiceTemplateAddRequest
                {
                    ClientId = 100,
                    ContractorId = 1,
                    UserAccountId = 1,
                    CurrencyId = 1,
                    TemplateName = "TestTemplateAdd",
                    NumberingId = 1
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, add);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidContractor()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddInvoiceTemplateAbl(db._repository);

                var add = new InvoiceTemplateAddRequest
                {
                    ClientId = 1,
                    ContractorId = 100,
                    UserAccountId = 1,
                    CurrencyId = 1,
                    TemplateName = "TestTemplateAdd",
                    NumberingId = 1
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, add);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddInvoiceTemplateAbl(db._repository);

                var add = new InvoiceTemplateAddRequest
                {
                    ClientId = 1,
                    ContractorId = 1,
                    UserAccountId = 100,
                    CurrencyId = 1,
                    TemplateName = "TestTemplateAdd",
                    NumberingId = 1
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, add);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnOutOfPossesionClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddInvoiceTemplateAbl(db._repository);

                var add = new InvoiceTemplateAddRequest
                {
                    ClientId = 1,
                    ContractorId = 1,
                    UserAccountId = 1,
                    CurrencyId = 1,
                    TemplateName = "TestTemplateAdd",
                    NumberingId = 1
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(2, add);
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