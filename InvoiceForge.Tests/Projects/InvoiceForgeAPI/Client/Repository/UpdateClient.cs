

using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using Xunit;

namespace Repository
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
                        Name = tClient.Name,
                        IN = tClient.IN,
                        TIN = tClient.TIN,
                        Mobil = tClient.Mobil,
                        Tel = tClient.Tel,
                        Email = tClient.Email
                    };

                    var updateClientResult = await db._repository.Client.Update(clientToCompare.Id, updateClient, tClient.Type);

                    await db._repository.Save();
                    Assert.True(updateClientResult);

                    var updatedClient = await db._context.Client.FindAsync(clientToCompare.Id);
                    Assert.NotNull(updatedClient);

                    if (updatedClient is not null)
                    {
                        Assert.Equal(1, updateClient.AddressId);
                        Assert.Equal(tClient.Name, updateClient.Name);
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

        [Fact]
        public Task ThrowErrorWhenUpdateIdenticValues()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var tclient = await db._context.Client.FindAsync(1);

                //ASSERT
                Assert.NotNull(tclient);
                var updateClient = new ClientUpdateRequest
                {
                    AddressId = tclient!.AddressId,
                    Name = tclient.Name,
                    IN = tclient.IN,
                    TIN = tclient.TIN,
                    Mobil = tclient.Mobil,
                    Tel = tclient.Tel,
                    Email = tclient.Email,
                    Owner = tclient.Owner
                };

                try
                {
                    var result = await db._repository.Client.Update(1, updateClient, tclient.Type);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}