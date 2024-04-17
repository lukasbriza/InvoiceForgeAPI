

using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace ClientRepository
{
    [Collection("Sequential")]
    public class UpdateClient: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateClientById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var clientToCompare = await db._repository.Client.GetById(1);

                //ASSERT
                Assert.NotNull(clientToCompare);
                
                if (clientToCompare is not null)
                {
                    var tClient = new TestClient();
                    var updateClient = new ClientUpdateRequest
                    {
                        Owner = clientToCompare.Owner,
                        AddressId = 1,
                        ClientName = tClient.ClientName,
                        IN = tClient.IN,
                        TIN = tClient.TIN,
                        Mobil = tClient.Mobil,
                        Tel = tClient.Tel,
                        Email = tClient.Email
                    };

                    var updateClientResult = await db._repository.Client.Update(clientToCompare.Id, updateClient, null);

                    await db._repository.Save();
                    Assert.True(updateClientResult);

                    var updatedClient = await db._context.Client.FindAsync(clientToCompare.Id);
                    Assert.NotNull(updatedClient);

                    if (updatedClient is not null)
                    {
                        Assert.Equal(1, updateClient.AddressId);
                        Assert.Equal(tClient.ClientName, updateClient.ClientName);
                        Assert.Equal(tClient.IN, updateClient.IN);
                        Assert.Equal(tClient.TIN, updateClient.TIN);
                        Assert.Equal(tClient.Mobil, updateClient.Mobil);
                        Assert.Equal(tClient.Tel, updateClient.Tel);
                        Assert.Equal(tClient.Email, updateClient.Email);
                    }
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}