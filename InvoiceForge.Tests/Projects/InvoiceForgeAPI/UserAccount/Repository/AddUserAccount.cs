using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace UserAccountRepository
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
                db.InitializeDbForTest();

                //ASSERT
                var addUserAccount = new UserAccountAddRequest
                {
                    BankId = 1,
                    AccountNumber = "TestAccountNumber",
                    IBAN = "TestIBAN"
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