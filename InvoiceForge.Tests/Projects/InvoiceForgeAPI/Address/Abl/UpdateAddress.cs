using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.address;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
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
                await db.InitInvoiceCopies();
                
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

                var copyAddress = await db._context.InvoiceAddressCopy.Where(a => a.OriginId == 1 && a.Owner == 1).ToListAsync();
                
                var invoices = new List<Invoice>();

                copyAddress?.ForEach(address => {
                    Assert.True(address.Outdated);

                    var addressId = address.Id;
                    var entityCopies =  db._context.InvoiceEntityCopy
                        .Where(c => c.Owner == 1 && c.AddressCopyId == addressId)
                        .ToList();
                    
                        entityCopies.ForEach( entity => {
                            Assert.True(entity.Outdated);

                            if (entity.OriginClientId is not null){

                                var i = db._context.Invoice.Where(i => i.ClientCopyId == entity.Id).ToList();
                                invoices.AddRange(i);
                            }
                            if (entity.OriginContractorId is not null)
                            {
                                var i = db._context.Invoice.Where(i => i.ContractorCopyId == entity.Id).ToList();
                                invoices.AddRange(i);
                            }
                        });
                });
                Assert.True(invoices.Count > 0);
                
                invoices.ForEach(i => {
                    Assert.True(i.Outdated);
                });

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
                
                await db.InitInvoiceCopies();
                var abl = new UpdateAddressAbl(db._repository);
                var tAddress= new TestAddress();

                //ASSERT
                var address = new AddressUpdateRequest{
                    Owner = 1000,
                    CountryId = tAddress.CountryId,
                    Street = tAddress.Street,
                    StreetNumber = tAddress.StreetNumber,
                    City = tAddress.City,
                    PostalCode = tAddress.PostalCode,
                };

                try
                {
                    var result = await abl.Resolve(1, address);
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
        public Task ThrowErrorOnIvalidAddressId()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                await db.InitInvoiceCopies();
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

                try
                {
                    var result = await abl.Resolve(1000, address);
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
        public Task ThrowErrorOnInvalidCountryId()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                await db.InitInvoiceCopies();
                var abl = new UpdateAddressAbl(db._repository);
                var tAddress= new TestAddress();

                //ASSERT
                var address = new AddressUpdateRequest{
                    Owner = 1,
                    CountryId = 1000,
                    Street = tAddress.Street,
                    StreetNumber = tAddress.StreetNumber,
                    City = tAddress.City,
                    PostalCode = tAddress.PostalCode,
                };

                try
                {
                    var result = await abl.Resolve(1, address);
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
        public Task ThrowErrorOnNonEqualOwnerAndUserId()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                await db.InitInvoiceCopies();
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

                try
                {
                    var result = await abl.Resolve(2, address);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorWhenProvideIdenticAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                

                var abl = new UpdateAddressAbl(db._repository);
                
                var tAddress = await db._context.Address.FindAsync(1);
                Assert.NotNull(tAddress);

                //ASSERT
                var address = new AddressUpdateRequest{
                    Owner = 1,
                    CountryId = tAddress.CountryId,
                    Street = tAddress.Street,
                    StreetNumber = tAddress.StreetNumber,
                    City = tAddress.City,
                    PostalCode = tAddress.PostalCode,
                };

                try
                {
                    var result = await abl.Resolve(1, address);
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