using FunctionalTests.Projects.InvoiceForgeApi;

using InvoiceForgeApi.Errors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class RemoveUserAccount: WebApplicationFactory
    {
        [Fact]
        public Task CanRemoveUserAccountById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper(false);
                db.InitUsers();
                db.InitBanks();
                db.InitUserAccounts();
                var dbUserAccounts = await db._context.UserAccount.Select(a => a.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(dbUserAccounts);
                Assert.IsType<List<int>>(dbUserAccounts);

                async Task call(int accountId){
                    var removeResult = await db._repository.UserAccount.Delete(accountId);
                }
                dbUserAccounts.ForEach(accountId => {
                    var task = call(accountId);
                    task.Wait();
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
                
                //ASSERT
                try
                {
                    var removeResult = await db._repository.UserAccount.Delete(100);
                }
                catch (Exception error)
                {
                    Assert.IsType<NoEntityError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}