
using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FunctionalTests.Projects.InvoiceForgeAPI.Address.Repository
{
    [Collection("Sequential")]
    public class GetAddress: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfAddressesForUser()
        {
            return RunTest(async (clientm) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var users = await db._context.User.Select(u => u.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(users);
                Assert.IsType<List<int>>(users);

                users.ForEach(async userId => {
                    var addressesValidation = new AddressSeed().Populate().FindAll(a => a.Owner == userId);
                    var addresses = await db._repository.Address.GetAll(userId);
                    
                    Assert.NotNull(addresses);
                    Assert.IsType<List<AddressGetRequest>>(addresses);
                    if (addresses is not null)
                    {
                        Assert.Equal(addressesValidation.Count, addresses.Count);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnRightAddressById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var addresses = await db._context.Address.Select(a => a.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(addresses);
                Assert.IsType<List<int>>(addresses);

                addresses.ForEach(async addressId => {
                    var repoAddress = await db._repository.Address.GetById(addressId);
                    Assert.NotNull(repoAddress);

                    if (repoAddress is not null)
                    {
                        Assert.IsType<AddressGetRequest>(repoAddress);
                        Assert.Equal(repoAddress.Id, addressId);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}