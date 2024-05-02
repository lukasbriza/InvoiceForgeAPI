using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class UpdateUserAccount: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateUserAccountById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                

                //ASSERT
                var updateUserAccount = new UserAccountUpdateRequest
                {
                    Owner = 1,
                    BankId = 2,
                    AccountNumber = "TestNumber",
                    IBAN = "TestIBAN"
                };

                var updateUserAccountResult = await db._repository.UserAccount.Update(1, updateUserAccount);

                await db._repository.Save();
                Assert.True(updateUserAccountResult);

                var updatedUserAccount = await db._context.UserAccount.FindAsync(1);
                Assert.NotNull(updatedUserAccount);
                Assert.IsType<UserAccount>(updatedUserAccount);

                if (updatedUserAccount is not null)
                {
                    Assert.Equal(updatedUserAccount.AccountNumber, updateUserAccount.AccountNumber);
                    Assert.Equal(updatedUserAccount.Owner, updateUserAccount.Owner);
                    Assert.Equal(updatedUserAccount.BankId, updateUserAccount.BankId);
                    Assert.Equal(updatedUserAccount.IBAN, updatedUserAccount.IBAN);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorWhenUpdateNonExistentUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                

                //ASSERT
                var entity = new UserAccountUpdateRequest{ AccountNumber = "TestAccountNumber", IBAN = "TestIBAN" };
                try
                {
                    var updateResult = await db._repository.UserAccount.Update(100, entity);
                }
                catch (Exception error)
                {
                    Assert.IsType<DatabaseCallError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorWhenUpdateIdenticValues()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var userAccount = await db._context.UserAccount.FindAsync(1);

                //ASSERT
                Assert.NotNull(userAccount);
                var updateUserAccount = new UserAccountUpdateRequest
                {
                    BankId = userAccount.BankId,
                    AccountNumber = userAccount.AccountNumber,
                    IBAN = userAccount.IBAN
                };

                try
                {
                    var result = await db._repository.UserAccount.Update(1, updateUserAccount);
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