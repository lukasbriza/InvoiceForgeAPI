using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.contractor;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class AddContractor: WebApplicationFactory
    {
        [Fact]
        public Task CanAddContractor()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var abl = new AddContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var addContractor = new ContractorAddRequest {
                    AddressId = 1,
                    TypeId = 1,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Email = tContractor.Email,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Www = tContractor.Www,
                };

                //ASSERT
                var result = await abl.Resolve(1, addContractor);
                var control = await db._context.Contractor.Where(a =>
                    a.AddressId == addContractor.AddressId &&
                    a.Name == addContractor.Name &&
                    a.IN == addContractor.IN &&
                    a.TIN == addContractor.TIN &&
                    a.Tel == addContractor.Tel &&
                    a.Email == addContractor.Email &&
                    a.Mobil == addContractor.Mobil
                ).ToListAsync();

                Assert.True(result);
                Assert.NotNull(control);
                Assert.True(control.Count() == 1);
                Assert.Equal(control[0].AddressId, addContractor.AddressId);
                Assert.Equal(control[0].Name, addContractor.Name);
                Assert.Equal(control[0].IN, addContractor.IN);
                Assert.Equal(control[0].TIN, addContractor.TIN);
                Assert.Equal(control[0].Email, addContractor.Email);
                Assert.Equal(control[0].Mobil, addContractor.Mobil);
                Assert.Equal(control[0].Tel, addContractor.Tel);
                Assert.Equal(control[0].Www, addContractor.Www);

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
                var abl = new AddContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var addContractor = new ContractorAddRequest {
                    AddressId = 1,
                    TypeId = 1,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Email = tContractor.Email,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Www = tContractor.Www,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(100, addContractor);
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
                var abl = new AddContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var addContractor = new ContractorAddRequest {
                    AddressId = 100,
                    TypeId = 1,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Email = tContractor.Email,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Www = tContractor.Www,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, addContractor);
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
                var abl = new AddContractorAbl(db._repository);
                var tContractor = new TestContractor();

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

                var addContractor = new ContractorAddRequest {
                    AddressId = addressId,
                    TypeId = 1,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Email = tContractor.Email,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Www = tContractor.Www,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, addContractor);
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
                var abl = new AddContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var addContractor = new ContractorAddRequest {
                    AddressId = 1,
                    TypeId = 100,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Email = tContractor.Email,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Www = tContractor.Www,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, addContractor);
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