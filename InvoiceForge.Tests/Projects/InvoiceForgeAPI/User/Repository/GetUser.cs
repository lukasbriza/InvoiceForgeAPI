using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class GetUser: WebApplicationFactory
    {
        [Fact]
        public Task CanGetUserPlainById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var usersIds = await db._context.User.Select(u => u.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(usersIds);
                Assert.IsType<List<int>>(usersIds);

                async Task call(int id){
                    var userValidation = new UserSeed().Populate().Find(u => u.Id == id);
                    var user = await db._repository.User.GetById(id, true);

                    Assert.NotNull(user);
                    Assert.IsType<UserGetRequest>(user);
                    if (user is not null)
                    {
                        Assert.Equal(id, user.Id);
                    }
                }
                
                usersIds.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task GetUserFullById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper(false);
                
                var usersIds = await db._context.User.Select(u => u.Id).ToListAsync();
                var clients = new ClientSeed().Populate();
                var userAccounts = new ContractorSeed().Populate();
                var addresses = new AddressSeed().Populate();
                var invoiceItems = new InvoiceItemSeed().Populate();

                //ASSERT
                async Task call(int userId){
                    var userClients = clients.FindAll(c => c.Owner == userId);
                    var userUserAccounts = userAccounts.FindAll(a => a.Owner == userId);
                    var userAddresses = addresses.FindAll(a => a.Owner == userId);
                    var userInvoiceItems = invoiceItems.FindAll(i => i.Owner == userId);

                    var dbUser = await db._repository.User.GetById(userId, false);

                    Assert.NotNull(dbUser);
                    Assert.IsType<UserGetRequest>(dbUser);
                    Assert.Equal(dbUser?.Clients?.Count(), userClients.Count);
                    Assert.Equal(dbUser?.UserAccounts?.Count(), userUserAccounts.Count);
                    Assert.Equal(dbUser?.Addresses?.Count(), userAddresses.Count);
                    Assert.Equal(dbUser?.InvoiceItems?.Count(), userInvoiceItems.Count);
                }

                usersIds.ForEach(userId => {
                    var task = call(userId);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }
        [Fact]
        public Task ReturnNullOnNonExistentUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                //ASSERT
                var nonExistentUser = await db._repository.User.GetById(100);
                Assert.Null(nonExistentUser);
                
                //CLEAN
                db.Dispose();
            });
        }
    }
}