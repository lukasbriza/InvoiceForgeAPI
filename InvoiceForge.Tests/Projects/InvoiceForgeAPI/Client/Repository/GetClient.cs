using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientRepository
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
                db.InitializeDbForTest();
                var users = await db._context.User.Select(u => u.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(users);
                Assert.IsType<List<int>>(users);

                users.ForEach(async userId => {
                    var clientValidation = new ClientSeed().Populate().FindAll(c => c.Owner == userId);
                    var clients = await db._repository.Client.GetAll(userId);

                    Assert.NotNull(clients);
                    Assert.IsType<List<ClientGetRequest>>(clients);
                    if (clients is not null)
                    {
                        Assert.Equal(clientValidation.Count, clients.Count);
                    }
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
                db.InitializeDbForTest();
                var clients = await db._context.Client.Select(c => c.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(clients);
                Assert.IsType<List<int>>(clients);
                
                clients.ForEach(async clientId => {
                    var repoClient = await db._repository.Client.GetById(clientId);
                    Assert.NotNull(repoClient);

                    if (repoClient is not null)
                    {
                        Assert.IsType<ClientGetRequest>(repoClient);
                        Assert.Equal(repoClient.Id, clientId);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}