using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Abl.address;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class DeleteAddress: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteAddressAbl(db._repository);
                
                var contractorAddressIds = await db._context.Contractor.Select(c => new {c.Id, c.AddressId}).ToListAsync();
                var clientAddressIds = await db._context.Client.Select(c => new {c.Id, c.AddressId}).ToListAsync();
                var ids = await db._context.Address
                    .Where(a => 
                        !contractorAddressIds.Select(c => c.AddressId).Contains(a.Id) ||
                        !clientAddressIds.Select(c => c.AddressId).Contains(a.Id)
                    )
                    .Select(a => a.Id).ToListAsync();


                //ASSERT
                ids.ForEach(async id => {
                    var result = await abl.Resolve(id);
                    Assert.True(result);
                });

                ids.ForEach(async id => {
                    var control = await db._context.Address.FindAsync(id);
                    Assert.Null(control);
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDeleteLinkedAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var abl = new DeleteAddressAbl(db._repository);

                //ASSERT
                try
                {
                    var reuslt = await abl.Resolve(1);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                var contractors = await db._context.Contractor.Select(c => c.Id).ToListAsync();
                contractors.ForEach(async id => {
                    var result = await db._repository.Contractor.Delete(id);
                    Assert.True(result);
                });

                try
                {
                    var reuslt = await abl.Resolve(1);
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
        public Task ThrowErrorOnDeleteNonExistentAddress()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                
                var abl = new DeleteAddressAbl(db._repository);
                
                //ASSERT
                try
                {
                    var result = await abl.Resolve(100);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
        
    }
}