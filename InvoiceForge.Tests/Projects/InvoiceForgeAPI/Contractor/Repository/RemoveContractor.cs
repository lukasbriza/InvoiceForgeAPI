using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Errors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Repository
{
    [Collection("Sequential")]
    public class RemoveContractor: WebApplicationFactory
    {
        [Fact]
        public Task CanRemoveContractorById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper(false);
                db.InitCountries();
                db.InitUsers();
                db.InitAddresses();
                db.InitContractors();
                var dbContractorsIds = await db._context.Contractor.Select(c => c.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(dbContractorsIds);
                Assert.IsType<List<int>>(dbContractorsIds);

                dbContractorsIds.ForEach(async contractorId => {
                    var removeResult = await db._repository.Contractor.Delete(contractorId);
                    Assert.True(removeResult);
                });

                await db._repository.Save();

                dbContractorsIds.ForEach( contractorId => {
                    var deletedContractor = db._context.Contractor.Find(contractorId);
                    Assert.Null(deletedContractor);
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnRemoveNonExistentContractor()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitCountries();
                db.InitUsers();
                db.InitAddresses();
                db.InitContractors();

                //ASSERT
                try
                {
                    var removeResult = await db._repository.Contractor.Delete(100);
                    await db._repository.Save();
                }
                catch (Exception error)
                {
                    Assert.IsType<NoEntityError>(error);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}