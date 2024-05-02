using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.address;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
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
                
                var abl = new AddAddressAbl(db._repository);
                
                //ASSERT
                var tAddres = new TestAddress();
                var addAddress = new AddressAddRequest
                {
                    CountryId = tAddres.CountryId,
                    Street = tAddres.Street,
                    StreetNumber = tAddres.StreetNumber,
                    City = tAddres.City,
                    PostalCode = tAddres.PostalCode
                };
                var result = await abl.Resolve(1, addAddress);

                var addressControl = await db._context.Address.Where(a => 
                    a.Street == tAddres.Street && 
                    a.StreetNumber == tAddres.StreetNumber &&
                    a.City == tAddres.City &&
                    a.PostalCode == tAddres.PostalCode &&
                    a.CountryId == tAddres.CountryId
                ).ToListAsync();

                Assert.True(result); 
                Assert.NotNull(addressControl);
                Assert.True(addressControl.Count() == 1); 
                Assert.Equal(addressControl[0].CountryId, addAddress.CountryId);
                Assert.Equal(addressControl[0].Street, addAddress.Street);  
                Assert.Equal(addressControl[0].StreetNumber, addAddress.StreetNumber);
                Assert.Equal(addressControl[0].City, addAddress.City); 
                Assert.Equal(addressControl[0].PostalCode, addAddress.PostalCode);

                //CLEAN
                db.Dispose(); 
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidCountry()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var abl = new AddAddressAbl(db._repository);
                
                //ASSERT
                var tAddres = new TestAddress();
                var addAddress = new AddressAddRequest
                {
                    CountryId = 1000,
                    Street = tAddres.Street,
                    StreetNumber = tAddres.StreetNumber,
                    City = tAddres.City,
                    PostalCode = tAddres.PostalCode
                };

                try
                {
                    var result = await abl.Resolve(1,addAddress);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var abl = new AddAddressAbl(db._repository);
                
                //ASSERT
                var tAddres = new TestAddress();
                var addAddress = new AddressAddRequest
                {
                    CountryId = tAddres.CountryId,
                    Street = tAddres.Street,
                    StreetNumber = tAddres.StreetNumber,
                    City = tAddres.City,
                    PostalCode = tAddres.PostalCode
                };

                try
                {
                    var result = await abl.Resolve(1000, addAddress);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDuplicitAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var abl = new AddAddressAbl(db._repository);
                var tAddres = new TestAddress();
                var addAddress = new AddressAddRequest
                {
                    CountryId = tAddres.CountryId,
                    Street = tAddres.Street,
                    StreetNumber = tAddres.StreetNumber,
                    City = tAddres.City,
                    PostalCode = tAddres.PostalCode
                };
                await abl.Resolve(1, addAddress);

                try
                {
                    var result = await abl.Resolve(1, addAddress);
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