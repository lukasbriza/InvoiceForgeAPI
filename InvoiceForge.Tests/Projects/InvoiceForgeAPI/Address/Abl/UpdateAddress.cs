using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.address;
using InvoiceForgeApi.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AddressAbl
{
    [Collection("Sequential")]
    public class UpdateAddress: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var abl = new UpdateAddressAbl(db._repository);
                var tAddress= new TestAddress();

                //ASSERT
                var address = new AddressUpdateRequest{
                    Owner = 1,
                    CountryId = tAddress.CountryId,
                    Street = tAddress.Street,
                    StreetNumber = tAddress.StreetNumber,
                    City = tAddress.City,
                    PostalCode = tAddress.PostalCode,
                };

                var result = await abl.Resolve(1, address);

                Assert.True(result);

                var controlAddress = await db._context.Address.FindAsync(1);

                Assert.NotNull(controlAddress);
                if (controlAddress is not null){
                    Assert.Equal(address.CountryId, controlAddress.CountryId);
                    Assert.Equal(address.Street, controlAddress.Street);
                    Assert.Equal(address.StreetNumber, controlAddress.StreetNumber);
                    Assert.Equal(address.City, controlAddress.City);
                    Assert.Equal(address.PostalCode, controlAddress.PostalCode);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}