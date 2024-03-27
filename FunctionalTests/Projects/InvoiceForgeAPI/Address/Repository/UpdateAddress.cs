using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace AddressRepository
{
    [Collection("Sequential")]
    public class UpdateAddress: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateAddressById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var addressToCompare = await db._repository.Address.GetById(1);

                //ASSERT
                Assert.NotNull(addressToCompare);

                if (addressToCompare is not null)
                {
                    var updateAddress = new AddressUpdateRequest
                    {
                        Owner = addressToCompare.Owner,
                        Street = "TestStreet",
                        StreetNumber = 1000,
                        City = "TestCity",
                        PostalCode = 123456789
                    };

                    var updateAddressResult = await db._repository.Address.Update(addressToCompare.Id,updateAddress);
                    
                    await db._repository.Save();
                    Assert.True(updateAddressResult);

                    var updatedAddress = await db._context.Address.FindAsync(addressToCompare.Id);
                    Assert.NotNull(updatedAddress);

                    if (updatedAddress is not null)
                    {
                        Assert.Equal("TestStreet", updatedAddress.Street);
                        Assert.Equal(1000, updatedAddress.StreetNumber);
                        Assert.Equal("TestCity", updatedAddress.City);
                        Assert.Equal(123456789, updatedAddress.PostalCode);
                    }
                }
                
                //CLEAN
                db.Dispose();
            });
        }
    }
}