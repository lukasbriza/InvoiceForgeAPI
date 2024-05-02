using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.contractor;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class UpdateContractor: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateContractor()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                await db.InitInvoiceCopies();

                var abl = new UpdateContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var updateContractor = new ContractorUpdateRequest
                {
                    Owner = 1,
                    AddressId = 1,
                    TypeId = 1,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Email = tContractor.Email,
                };

                //ASSERT
                var resolve = await abl.Resolve(1, updateContractor);
                Assert.True(resolve);

                var control = await db._context.Contractor.FindAsync(1);
                Assert.NotNull(control);
                if (control is not null)
                {
                    Assert.Equal(control.Owner, updateContractor.Owner);
                    Assert.Equal(control.AddressId, updateContractor.AddressId);
                    Assert.Equal(control.Name, updateContractor.Name);
                    Assert.Equal(control.IN, updateContractor.IN);
                    Assert.Equal(control.TIN, updateContractor.TIN);
                    Assert.Equal(control.Mobil, updateContractor.Mobil);
                    Assert.Equal(control.Tel, updateContractor.Tel);
                    Assert.Equal(control.Email, updateContractor.Email);
                }

                var linkedCopyEntities = await db._context.InvoiceEntityCopy.Where(e => e.OriginContractorId == 1).ToListAsync();
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
                var abl = new UpdateContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var updateContractor = new ContractorUpdateRequest
                    {
                        Owner = 100,
                        AddressId = 1,
                        TypeId = 1,
                        Name = tContractor.Name,
                        IN = tContractor.IN,
                        TIN = tContractor.TIN,
                        Mobil = tContractor.Mobil,
                        Tel = tContractor.Tel,
                        Email = tContractor.Email,
                    };
                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateContractor);
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
        public Task ThrowErrorOnInvalidContractorId()
        {
            return RunTest(async (client) =>{
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var updateContractor = new ContractorUpdateRequest
                    {
                        Owner = 1,
                        AddressId = 1,
                        TypeId = 1,
                        Name = tContractor.Name,
                        IN = tContractor.IN,
                        TIN = tContractor.TIN,
                        Mobil = tContractor.Mobil,
                        Tel = tContractor.Tel,
                        Email = tContractor.Email,
                    };
                //ASSERT
                try
                {
                    var result = await abl.Resolve(100, updateContractor);
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
        public Task ThrowErrorWhenProvideContractorOutOfPossession()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateContractorAbl(db._repository);
                var tContractor = new Contractor
                {
                    Owner = 2,
                    AddressId = 1,
                    Type = ClientType.Entrepreneur,
                    Name = "TestContractorName",
                    IN = 123456789,
                    TIN = "CZ123456789",
                    Mobil = "+420774876504",
                    Email = "email@mail.cz"
                };

                var entity = await db._context.Contractor.AddAsync(tContractor);
                await db._context.SaveChangesAsync();

                var updateContractor = new ContractorUpdateRequest
                    {
                        Owner = 1,
                        AddressId = 1,
                        TypeId = 1,
                        Name = tContractor.Name,
                        IN = tContractor.IN,
                        TIN = tContractor.TIN,
                        Mobil = tContractor.Mobil,
                        Tel = tContractor.Tel,
                        Email = tContractor.Email,
                    };
                //ASSERT
                try
                {
                    var result = await abl.Resolve(entity.Entity.Id, updateContractor);
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
        public Task ThrowErrorOnInvalidAddress()
        {
            return RunTest(async (client)  => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateContractorAbl(db._repository);
                var tContractor = new TestContractor();
                var updateContractor = new ContractorUpdateRequest
                    {
                        Owner = 1,
                        AddressId = 100,
                        TypeId = 1,
                        Name = tContractor.Name,
                        IN = tContractor.IN,
                        TIN = tContractor.TIN,
                        Mobil = tContractor.Mobil,
                        Tel = tContractor.Tel,
                        Email = tContractor.Email,
                    };
                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateContractor);
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
            return RunTest(async (client)  => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateContractorAbl(db._repository);

                var address = new Address
                {
                    Owner = 2,
                    CountryId = 1,
                    Street = "StreetTest",
                    StreetNumber = 10,
                    City = "CityTest",
                    PostalCode = 1000
                };
                var entity = await db._context.Address.AddAsync(address);
                await db._context.SaveChangesAsync();

                var tContractor = new TestContractor();
                var updateContractor = new ContractorUpdateRequest
                {
                    Owner = 1,
                    AddressId = entity.Entity.Id,
                    TypeId = 1,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Email = tContractor.Email,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateContractor);
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
        public Task ThrowErrorOnInvalidClientType()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateContractorAbl(db._repository);

                var tContractor = new TestContractor();
                var updateContractor = new ContractorUpdateRequest
                {
                    Owner = 1,
                    AddressId = 1,
                    TypeId = 10,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Email = tContractor.Email,
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, updateContractor);
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