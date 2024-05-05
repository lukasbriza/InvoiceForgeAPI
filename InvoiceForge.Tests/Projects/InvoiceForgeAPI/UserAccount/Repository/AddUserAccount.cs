using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
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
                

                //ASSERT
                var tAccount = new TestUserAccount();
                var addUserAccount = new UserAccountAddRequest
                {
                    BankId = tAccount.BankId,
                    AccountNumber = tAccount.AccountNumber,
                    IBAN = tAccount.IBAN
                };

                var addUserAccountResult = await db._repository.UserAccount.Add(1, addUserAccount);
                Assert.NotNull(addUserAccountResult);

                if (addUserAccountResult is not null)
                {
                    Assert.IsType<int>(addUserAccountResult);
                    var newUserAccount = await db._context.UserAccount.FindAsync(addUserAccountResult);

                    Assert.Equal(addUserAccount.AccountNumber, newUserAccount?.AccountNumber);
                    Assert.Equal(addUserAccount.BankId, newUserAccount?.BankId);
                    Assert.Equal(addUserAccount.IBAN, newUserAccount?.IBAN);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}