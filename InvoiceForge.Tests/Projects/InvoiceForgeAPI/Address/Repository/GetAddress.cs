
using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class GetAddress: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfAddressesForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var users = await db._context.User.Select(u => u.Id).ToListAsync();
                
                //ASSERT
                Assert.NotNull(users);
                Assert.IsType<List<int>>(users);

                async Task call(int userId){
                    var addressesValidation = new AddressSeed().Populate().FindAll(a => a.Owner == userId);
                    var addresses = await db._repository.Address.GetAll(userId);
                    
                    Assert.NotNull(addresses);
                    Assert.IsType<List<AddressGetRequest>>(addresses);
                    if (addresses is not null)
                    {
                        Assert.Equal(addressesValidation.Count, addresses.Count);
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
        public Task ReturnRightAddressById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var addresses = await db._context.Address.Select(a => a.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(addresses);
                Assert.IsType<List<int>>(addresses);

                async Task call(int addressId){
                    var repoAddress = await db._repository.Address.GetById(addressId);
                    Assert.NotNull(repoAddress);

                    if (repoAddress is not null)
                    {
                        Assert.IsType<AddressGetRequest>(repoAddress);
                        Assert.Equal(repoAddress.Id, addressId);
                    }
                }

                addresses.ForEach(addressId => {
                    var task = call(addressId);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}