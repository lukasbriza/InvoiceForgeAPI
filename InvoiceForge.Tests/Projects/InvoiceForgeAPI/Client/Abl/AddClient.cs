using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.client;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class AddClient: WebApplicationFactory
    {
        [Fact]
        public Task CanAddClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var tClient = new TestClient();
                var abl = new AddClientAbl(db._repository);
                var addClient = new ClientAddRequest {
                    AddressId = 1,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email
                };

                //ASSERT
                var result = await abl.Resolve(1, addClient);

                var control = await db._context.Client.Where(a =>
                    a.AddressId == addClient.AddressId &&
                    a.Name == addClient.Name &&
                    a.IN == addClient.IN &&
                    a.TIN == addClient.TIN &&
                    a.Tel == addClient.Tel &&
                    a.Email == addClient.Email &&
                    a.Mobil == addClient.Mobil
                ).ToListAsync();
                
                Assert.True(result);
                Assert.NotNull(control);
                Assert.True(control.Count() == 1);
                Assert.Equal(control[0].AddressId, addClient.AddressId);
                Assert.Equal(control[0].Name, addClient.Name);
                Assert.Equal(control[0].IN, addClient.IN);
                Assert.Equal(control[0].TIN, addClient.TIN);
                Assert.Equal(control[0].Mobil, addClient.Mobil);
                Assert.Equal(control[0].Tel, addClient.Tel);
                Assert.Equal(control[0].Email, addClient.Email);

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

                var tClient = new TestClient();
                var abl = new AddClientAbl(db._repository);

                //ASSERT
                var addClient = new ClientAddRequest {
                    AddressId = tClient.AddressId,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email
                };

                try
                {
                    var result = await abl.Resolve(1000, addClient);
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

                var tClient = new TestClient();
                var abl = new AddClientAbl(db._repository);

                //ASSERT
                var addClient = new ClientAddRequest {
                    AddressId = 1000,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email
                };

                try
                {
                    var result = await abl.Resolve(1, addClient);
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
        public Task ThrowErrorWhenProvideUnownedAddressId()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var tClient = new TestClient();
                var abl = new AddClientAbl(db._repository);

                var addAddress = new Address
                {
                    Owner = 2,
                    CountryId = 1,
                    Street = "AddressStreetTest",
                    StreetNumber = 111,
                    City = "TestCity",
                    PostalCode = 10000
                };
                var addressAddResult = await db._context.Address.AddAsync(addAddress);
                await db._context.SaveChangesAsync();
                var addressId = addressAddResult.Entity.Id;


                //ASSERT
                var addClient = new ClientAddRequest {
                    AddressId = addressId,
                    TypeId = 1,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email
                };

                try
                {
                    var result = await abl.Resolve(1, addClient);
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
        public Task ThrowErrorOnInvalidTypeId()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var tClient = new TestClient();
                var abl = new AddClientAbl(db._repository);

                //ASSERT
                var addClient = new ClientAddRequest {
                    AddressId = 1,
                    TypeId = 10,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email
                };

                try
                {
                    var result = await abl.Resolve(1, addClient);
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