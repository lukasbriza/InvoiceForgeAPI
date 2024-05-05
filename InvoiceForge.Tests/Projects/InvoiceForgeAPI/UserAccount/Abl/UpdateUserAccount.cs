using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Abl.userAccount;

using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class UpdateUserAccount: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();
                var abl = new UpdateUserAccountAbl(db._repository);

                var update = new UserAccountUpdateRequest
                {
                    Owner = 1,
                    BankId = 3,
                    AccountNumber = "SomeTestNumber",
                    IBAN = "SomeTestIban"
                };

                var result = await abl.Resolve(1, update);

                //ASSERT
                Assert.True(result);

                var control = await db._context.UserAccount.FindAsync(1);
                Assert.NotNull(control);
                Assert.Equal(update.Owner, control!.Owner);
                Assert.Equal(update.BankId, control.BankId);
                Assert.Equal(update.AccountNumber, control.AccountNumber);
                Assert.Equal(update.IBAN, control.IBAN);

                var copy = await db._context.InvoiceUserAccountCopy.Where(i => i.OriginId == 1).ToListAsync();
                Assert.NotNull(copy);
                Assert.True(copy[0].Outdated);

                var invoices = await db._context.Invoice.Where(i => i.UserAccountCopyId == copy[0].Id).ToListAsync();
                Assert.NotNull(invoices);
                invoices.ForEach(invoice => {
                    Assert.True(invoice.Outdated);
                });

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
                var abl = new UpdateUserAccountAbl(db._repository);

                var update = new UserAccountUpdateRequest
                {
                    Owner = 100,
                    BankId = 3,
                    AccountNumber = "SomeTestNumber",
                    IBAN = "SomeTestIban"
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
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
        public Task ThrowerrorOnInvalidUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateUserAccountAbl(db._repository);

                var update = new UserAccountUpdateRequest
                {
                    Owner = 1,
                    BankId = 3,
                    AccountNumber = "SomeTestNumber",
                    IBAN = "SomeTestIban"
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(100, update);
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
        public Task ThrowErrorOnDuplicitIban()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateUserAccountAbl(db._repository);

                var update = new UserAccountUpdateRequest
                {
                    Owner = 1,
                    BankId = 3,
                    AccountNumber = "SomeTestNumber",
                    IBAN = "SomeTestIban"
                };

                var prepare = await db._context.UserAccount.FindAsync(1);
                Assert.NotNull(prepare);
                prepare!.IBAN = update.IBAN;
                await db._context.SaveChangesAsync();

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NotUniqueEntityError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDuplicitAccountNumber()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateUserAccountAbl(db._repository);

                var update = new UserAccountUpdateRequest
                {
                    Owner = 1,
                    BankId = 3,
                    AccountNumber = "SomeTestNumber",
                    IBAN = "SomeTestIban"
                };

                var prepare = await db._context.UserAccount.FindAsync(1);
                Assert.NotNull(prepare);
                prepare!.AccountNumber = update.AccountNumber;
                await db._context.SaveChangesAsync();

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NotUniqueEntityError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidBank()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateUserAccountAbl(db._repository);

                var update = new UserAccountUpdateRequest
                {
                    Owner = 1,
                    BankId = 300,
                    AccountNumber = "SomeTestNumber",
                    IBAN = "SomeTestIban"
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
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