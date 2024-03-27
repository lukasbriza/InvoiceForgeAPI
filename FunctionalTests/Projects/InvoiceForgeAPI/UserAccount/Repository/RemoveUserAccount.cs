using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UserAccountRepository
{
    [Collection("Sequential")]
    public class RemoveUserAccount: WebApplicationFactory
    {
        [Fact]
        public Task CanRemoveUserAccountById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitUsers();
                db.InitBanks();
                db.InitUserAccounts();
                var dbUserAccounts = await db._context.UserAccount.Select(a => a.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(dbUserAccounts);
                Assert.IsType<List<int>>(dbUserAccounts);

                dbUserAccounts.ForEach(async accountId => {
                    var removeResult = await db._repository.UserAccount.Delete(accountId);
                });

                await db._repository.Save();

                dbUserAccounts.ForEach(userAccountId => {
                    var deletedUserAccount = db._context.UserAccount.Find(userAccountId);
                    Assert.Null(deletedUserAccount);
                });

                //CLEAN
                db.Dispose();
            });
        }
        
        [Fact]
        public Task ThrowErrorOnDeleteNonExistentUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                try
                {
                    var removeResult = await db._repository.UserAccount.Delete(100);
                }
                catch (Exception error)
                {
                    Assert.IsType<DatabaseCallError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}