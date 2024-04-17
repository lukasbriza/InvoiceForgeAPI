using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.Enum;
using Xunit;

namespace ClientRepository
{
    [Collection("Sequential")]
    public class AddClient: WebApplicationFactory
    {
        [Fact]
        public Task CanAddClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                var tClient = new TestClient();
                var addClient = new ClientAddRequest
                {
                    AddressId = 1,
                    TypeId = 1,
                    ClientName = tClient.ClientName,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email
                };

                var addClientResult = await db._repository.Client.Add(1, addClient, ClientType.LegalEntity);
                Assert.NotNull(addClientResult);

                if (addClientResult is not null)
                {
                    Assert.IsType<int>(addClientResult);
                    var newClient = await db._context.Client.FindAsync(addClientResult);

                    Assert.Equal(addClient.AddressId, newClient?.AddressId);
                    Assert.Equal(ClientType.LegalEntity, newClient?.Type);
                    Assert.Equal(addClient.ClientName, newClient?.ClientName);
                    Assert.Equal(addClient.IN, newClient?.IN);
                    Assert.Equal(addClient.TIN, newClient?.TIN);
                    Assert.Equal(addClient.Mobil, newClient?.Mobil);
                    Assert.Equal(addClient.Tel, newClient?.Tel);
                    Assert.Equal(addClient.Email, newClient?.Email);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}