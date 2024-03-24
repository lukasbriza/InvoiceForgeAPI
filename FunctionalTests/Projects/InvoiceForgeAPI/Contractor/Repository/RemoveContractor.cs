using FunctionalTests.Projects.InvoiceForgeApi;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FunctionalTests.Projects.InvoiceForgeAPI.Contractor.Repository
{
    [Collection("Sequential")]
    public class RemoveContractor: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateContractorById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
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
    }
}