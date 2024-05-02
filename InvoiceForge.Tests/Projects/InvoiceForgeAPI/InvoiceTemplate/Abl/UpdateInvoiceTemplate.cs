using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Abl.invoiceTemplate;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class UpdateInvoiceTemplate: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateInvoiceTemplate()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                //ASSERT
                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = entity!.NumberingId,
                    Owner = entity!.Owner,
                };

                var result = await abl.Resolve(1, update);
                Assert.True(result);

                var invoices = await db._context.Invoice.Where(i => i.TemplateId == 1).ToListAsync();
                Assert.NotNull(invoices);

                invoices?.ForEach(invoice => {
                    Assert.True(invoice.Outdated);
                });

                var control = await db._context.InvoiceTemplate.FindAsync(1);
                Assert.NotNull(control);
                Assert.Equal(update.TemplateName, control!.TemplateName);

                //CLEAN
                db.Dispose();

            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = entity!.NumberingId,
                    Owner = 100,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
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
        public Task ThrowErrorOnInvalidClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = 100,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = entity!.NumberingId,
                    Owner = entity!.Owner,
                };

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1, update);
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
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = 100,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = entity!.NumberingId,
                    Owner = entity!.Owner,
                };

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1, update);
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
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = 100,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = entity!.NumberingId,
                    Owner = entity!.Owner,
                };

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1, update);
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
        public Task ThrowErrorOnInvalidCurrency()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = 100,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = entity!.NumberingId,
                    Owner = entity!.Owner,
                };

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1, update);
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
        public Task ThrowErrorOnInvalidNumbering()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = 100,
                    Owner = entity!.Owner,
                };

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1, update);
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
        public Task ThrowErrorOnUpdateTemplateOutOfPossesion()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = "MyTestTemplateName",
                    NumberingId = entity!.NumberingId,
                    Owner = 2,
                };

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDuplicitNames()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceTemplateAbl(db._repository);

                var entity = await db._context.InvoiceTemplate.FindAsync(1);

                Assert.NotNull(entity);
                var update = new InvoiceTemplateUpdateRequest
                {
                    ClientId = entity!.ClientId,
                    ContractorId = entity!.ContractorId,
                    UserAccountId = entity!.UserAccountId,
                    CurrencyId = entity!.CurrencyId,
                    TemplateName = entity!.TemplateName,
                    NumberingId = entity!.NumberingId,
                    Owner = entity!.Owner,
                };

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1, update);
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