using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using Xunit;

namespace Repository
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
                
                var addressToCompare = await db._repository.Address.GetById(1);

                //ASSERT
                Assert.NotNull(addressToCompare);

                if (addressToCompare is not null)
                {
                    var tAddress = new TestAddress();
                    var updateAddress = new AddressUpdateRequest
                    {
                        CountryId = 1,
                        Owner = addressToCompare.Owner,
                        Street = tAddress.Street,
                        StreetNumber = tAddress.StreetNumber,
                        City = tAddress.City,
                        PostalCode = tAddress.PostalCode
                    };

                    var updateAddressResult = await db._repository.Address.Update(addressToCompare.Id,updateAddress);
                    
                    await db._repository.Save();
                    Assert.True(updateAddressResult);

                    var updatedAddress = await db._context.Address.FindAsync(addressToCompare.Id);
                    Assert.NotNull(updatedAddress);

                    if (updatedAddress is not null)
                    {
                        Assert.Equal(tAddress.Street, updatedAddress.Street);
                        Assert.Equal(tAddress.StreetNumber, updatedAddress.StreetNumber);
                        Assert.Equal(tAddress.City, updatedAddress.City);
                        Assert.Equal(tAddress.PostalCode, updatedAddress.PostalCode);
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
                var address = await db._context.Address.FindAsync(1);
                
                //ASSERT
                Assert.NotNull(address);
                var updateAddress = new AddressUpdateRequest {
                    Owner = address.Owner,
                    CountryId = address.CountryId,
                    Street = address.Street,
                    StreetNumber = address.StreetNumber,
                    City = address.City,
                    PostalCode = address.PostalCode
                };

                try
                {
                    var result = await db._repository.Address.Update(1, updateAddress);
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