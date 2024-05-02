using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class RemoveClient: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteClientById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper(false);
                db.InitCountries();
                db.InitUsers();
                db.InitAddresses();
                db.InitClients();
                var dbClientsIds = await db._context.Client.Select(c => c.Id).ToListAsync();
                
                //ASSERT
                Assert.NotNull(dbClientsIds);
                Assert.IsType<List<int>>(dbClientsIds);

                dbClientsIds.ForEach(async clientId => {
                    var removeResult = await db._repository.Client.Delete(clientId);
                    Assert.True(removeResult);
                });

                await db._repository.Save();

                dbClientsIds.ForEach( clientId => {
                    var deletedClient = db._context.Client.Find(clientId);
                    Assert.Null(deletedClient);
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}