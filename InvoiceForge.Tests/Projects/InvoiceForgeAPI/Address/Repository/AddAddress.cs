using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace AddressRepository
{
    [Collection("Sequential")]
    public class AddAddress: WebApplicationFactory
    {
        [Fact]
        public Task CanAddAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();

                //ASSERT
                var addAddress = new AddressAddRequest
                {
                    CountryId = 1,
                    Street = "TestStreet",
                    StreetNumber = 123456789,
                    City = "TestCity",
                    PostalCode = 123456789
                };

                var addAddressResult = await db._repository.Address.Add(1, addAddress);
                Assert.NotNull(addAddressResult);

                if (addAddressResult is not null)
                {
                    Assert.IsType<int>(addAddressResult);
                    var newAddress = await db._context.Address.FindAsync(addAddressResult);

                    Assert.Equal(addAddress.CountryId, newAddress?.CountryId);
                    Assert.Equal(addAddress.Street, newAddress?.Street);
                    Assert.Equal(addAddress.StreetNumber, newAddress?.StreetNumber);
                    Assert.Equal(addAddress.City, newAddress?.City);
                    Assert.Equal(addAddress.PostalCode, newAddress?.PostalCode);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}