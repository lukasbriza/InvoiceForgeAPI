
using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ContractorRepository
{
    [Collection("Sequential")]
    public class GetContractor: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfContractorsForUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var users = await db._context.User.Select(u => u.Id).ToListAsync();

                //ASSERT
                Assert.NotNull(users);
                Assert.IsType<List<int>>(users);

                users.ForEach(async userId => {
                    var contractorValidation = new ContractorSeed().Populate().FindAll(c => c.Owner == userId);
                    var contractors = await db._repository.Contractor.GetAll(userId);

                    Assert.NotNull(contractors);
                    Assert.IsType<List<ContractorGetRequest>>(contractors);
                    if (contractors is not null)
                    {
                        Assert.Equal(contractorValidation.Count, contractors.Count);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnRightContractorById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var contractors = await db._context.Contractor.Select(c => c.Id).ToListAsync();
                
                //ASSERT
                Assert.NotNull(contractors);
                Assert.IsType<List<int>>(contractors);

                contractors.ForEach(async contractorId => {
                    var repoContractor = await db._repository.Contractor.GetById(contractorId);
                    Assert.NotNull(repoContractor);

                    if(repoContractor is not null)
                    {
                        Assert.IsType<ContractorGetRequest>(repoContractor);
                        Assert.Equal(repoContractor.Id, contractorId);
                    }
                });
                
                //CLEAN
                db.Dispose();
            });
        }
    }
}