using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Abl.userAccount;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class AddUserAccount: WebApplicationFactory
    {
        [Fact]
        public Task CanAddUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new AddUserAccountAbl(db._repository);

                var userAccount = new UserAccountAddRequest
                {
                    BankId = 1,
                    AccountNumber = "TestNumber",
                    IBAN = "IBAN"
                };

                //ASSERT
                var result = await abl.Resolve(1, userAccount);
                Assert.True(result);

                var control = await db._context.UserAccount.Where(a => 
                    a.BankId == userAccount.BankId && 
                    a.AccountNumber == userAccount.AccountNumber &&
                    a.IBAN == userAccount.IBAN
                ).ToListAsync();

                Assert.NotNull(control);
                Assert.Equal(userAccount.BankId, control[0].BankId);
                Assert.Equal(userAccount.AccountNumber, control[0].AccountNumber);
                Assert.Equal(userAccount.IBAN, control[0].IBAN);

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
                var abl = new AddUserAccountAbl(db._repository);

                var userAccount = new UserAccountAddRequest
                {
                    BankId = 1,
                    AccountNumber = "TestNumber",
                    IBAN = "IBAN"
                };

                //ASSERT
                try
                {
                    var resolve = await abl.Resolve(100, userAccount);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLENA
                db.Dispose();
            });
        }

        [Fact]
        public async Task ThrowErrorOnDuplicitIbanOrAccountNumber()
        {
            //SETUP
            var db = new DatabaseHelper();
            var abl = new AddUserAccountAbl(db._repository);

            var userAccount = new UserAccountAddRequest
            {
                BankId = 1,
                AccountNumber = "TestNumber",
                IBAN = "IBAN"
            };

            var duplicitIban = new UserAccountAddRequest
            {
                BankId = 1,
                AccountNumber = "TestNumber1",
                IBAN = "IBAN"
            };

            var duplicitAccountNumber = new UserAccountAddRequest
            {
                BankId = 1,
                AccountNumber = "TestNumber",
                IBAN = "IBAN1"
            };

            var add = await db._repository.UserAccount.Add(1, userAccount);

            //ASSERT
            Assert.NotNull(add);

            try
            {
                var result = await abl.Resolve(1, duplicitIban);
            }
            catch (Exception ex)
            {
                Assert.IsType<ValidationError>(ex);
            }

            try
            {
                var result = await abl.Resolve(1, duplicitAccountNumber);
            }
            catch (Exception ex)
            {
                Assert.IsType<ValidationError>(ex);
            }

            //CLEAN
            db.Dispose();
        }
    }
}