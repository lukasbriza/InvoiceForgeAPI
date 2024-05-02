using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.client;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class UpdateClient: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();

                var abl = new UpdateClientAbl(db._repository);

                var tClient = new TestClient();
                var updateClient = new ClientUpdateRequest{
                    Owner = 1,
                    AddressId = 1,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email,
                };

                //ASSERT
                var resolve = await abl.Resolve(1, updateClient);
                Assert.True(resolve);

                var control = await db._context.Client.FindAsync(1);
                Assert.NotNull(control);
                if (control is not null)
                {
                    Assert.Equal(control.Owner, updateClient.Owner);
                    Assert.Equal(control.AddressId, updateClient.AddressId);
                    Assert.Equal(control.Name, updateClient.Name);
                    Assert.Equal(control.IN, updateClient.IN);
                    Assert.Equal(control.TIN, updateClient.TIN);
                    Assert.Equal(control.Mobil, updateClient.Mobil);
                    Assert.Equal(control.Tel, updateClient.Tel);
                    Assert.Equal(control.Email, updateClient.Email);
                }

                var linkedCopyEntities = await db._context.InvoiceEntityCopy.Where(e => e.OriginClientId == 1).ToListAsync();

                Assert.NotNull(linkedCopyEntities);

                linkedCopyEntities.ForEach(copy => {
                    Assert.True(copy.Outdated);
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
                var abl = new UpdateClientAbl(db._repository);
                var tClient = new TestClient();
                var updateClient = new ClientUpdateRequest{
                    Owner = 1000,
                    AddressId = 1,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateClient);
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
        public Task ThrowErrorOnInvalidAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateClientAbl(db._repository);
                var tClient = new TestClient();
                var updateClient = new ClientUpdateRequest{
                    Owner = 1,
                    AddressId = 1000,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateClient);
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
         public Task ThrowErrorOnAddressOutOfPossession()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateClientAbl(db._repository);
                var tClient = new TestClient();
                var updateClient = new ClientUpdateRequest{
                    Owner = 1,
                    AddressId = 3,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email,
                };

                var address = new Address{
                    CountryId = 1,
                    Owner = 2,
                    Street = "Street3",
                    StreetNumber = 12,
                    City = "City3",
                    PostalCode = 1212
                };

                await db._context.Address.AddAsync(address);
                await db._context.SaveChangesAsync();

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateClient);
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
        public Task ThrowErrorOnInvalidType()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateClientAbl(db._repository);
                var tClient = new TestClient();
                var updateClient = new ClientUpdateRequest{
                    Owner = 1,
                    AddressId = 1,
                    TypeId = 100,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateClient);
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