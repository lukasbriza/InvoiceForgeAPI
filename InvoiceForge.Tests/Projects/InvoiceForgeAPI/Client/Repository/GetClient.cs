using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class GetClient: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfClientsForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var users = await db._context.User.Select(u => u.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(users);
                Assert.IsType<List<int>>(users);

                async Task call(int userId){
                    var clientValidation = new ClientSeed().Populate().FindAll(c => c.Owner == userId);
                    var clients = await db._repository.Client.GetAll(userId);

                    Assert.NotNull(clients);
                    Assert.IsType<List<ClientGetRequest>>(clients);
                    if (clients is not null)
                    {
                        Assert.Equal(clientValidation.Count, clients.Count);
                    }
                }

                users.ForEach(userId => {
                    var task = call(userId);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnRightClientById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var clients = await db._context.Client.Select(c => c.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(clients);
                Assert.IsType<List<int>>(clients);
                
                async Task call(int clientId){
                    var repoClient = await db._repository.Client.GetById(clientId);
                    Assert.NotNull(repoClient);

                    if (repoClient is not null)
                    {
                        Assert.IsType<ClientGetRequest>(repoClient);
                        Assert.Equal(repoClient.Id, clientId);
                    }
                }
                
                clients.ForEach(clientId => {
                    var task = call(clientId);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}