

using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
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
                    var updateClient = new ClientUpdateRequest
                    {
                        Owner = clientToCompare.Owner,
                        AddressId = 2,
                        ClientName = "TestName",
                        IN = 123456789,
                        TIN = "TestTIN",
                        Mobil = "+420774876504",
                        Tel = "+420774876504",
                        Email = "TestMail"
                    };

                    var updateClientResult = await db._repository.Client.Update(clientToCompare.Id, updateClient, null);

                    await db._repository.Save();
                    Assert.True(updateClientResult);

                    var updatedClient = await db._context.Client.FindAsync(clientToCompare.Id);
                    Assert.NotNull(updatedClient);

                    if (updatedClient is not null)
                    {
                        Assert.Equal(2, updateClient.AddressId);
                        Assert.Equal("TestName", updateClient.ClientName);
                        Assert.Equal(123456789, updateClient.IN);
                        Assert.Equal("TestTIN", updateClient.TIN);
                        Assert.Equal("+420774876504", updateClient.Mobil);
                        Assert.Equal("+420774876504", updateClient.Tel);
                        Assert.Equal("TestMail", updateClient.Email);
                    }
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}