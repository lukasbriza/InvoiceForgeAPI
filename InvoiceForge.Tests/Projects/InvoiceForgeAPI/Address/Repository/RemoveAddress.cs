using FunctionalTests.Projects.InvoiceForgeApi;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class RemoveAddress: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteAddressById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper(false);
                db.InitCountries();
                db.InitUsers();
                db.InitAddresses();
                var dbAddressesIds = await db._context.Address.Select(a => a.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(dbAddressesIds);
                Assert.IsType<List<int>>(dbAddressesIds);
                
                dbAddressesIds.ForEach(async addressId => {
                    var removeResult = await db._repository.Address.Delete(addressId);
                    Assert.True(removeResult);
                });
                
                await db._repository.Save();
                
                dbAddressesIds.ForEach( addressId => {
                    var deletedAddress =  db._context.Address.Find(addressId);
                    Assert.Null(deletedAddress);
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}