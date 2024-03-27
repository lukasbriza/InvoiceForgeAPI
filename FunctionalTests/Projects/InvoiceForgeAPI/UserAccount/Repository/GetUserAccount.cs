using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UserAccountRepository
{
    [Collection("Sequential")]
    public class GetUserAccount: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfUserAccountsForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var users = await db._context.User.Select(u => u.Id).ToListAsync();
                
                //ASSERT
                Assert.NotNull(users);
                Assert.IsType<List<int>>(users);

                users.ForEach(async userId => {
                    var userAccountValidation = new UserAccountSeed().Populate().FindAll(a => a.Owner == userId);
                    var userAccounts = await db._repository.UserAccount.GetAll(userId, true);

                    Assert.NotNull(userAccounts);
                    Assert.IsType<List<UserAccountGetRequest>>(userAccounts);
                    if (userAccounts is not null)
                    {
                        Assert.Equal(userAccountValidation.Count, userAccounts.Count);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }
        [Fact]
        public Task GetUserAccountById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var userAccounts = await db._context.UserAccount.Select(a => a.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(userAccounts);
                Assert.IsType<List<int>>(userAccounts);

                userAccounts.ForEach(async userAccountId => {
                    var repoUserAccount = await db._repository.UserAccount.GetById(userAccountId);
                    Assert.NotNull(repoUserAccount);

                    if (repoUserAccount is not null)
                    {
                        Assert.IsType<UserAccountGetRequest>(repoUserAccount);
                        Assert.Equal(repoUserAccount.Id, userAccountId);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnNullOnNonExistentUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                var nonExistentUserAccount = await db._repository.UserAccount.GetById(100);   
                Assert.Null(nonExistentUserAccount);

                //CLEAN
                db.Dispose();
            });
        }
    }
}