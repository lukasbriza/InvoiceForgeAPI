using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace ContractorRepository
{
    [Collection("Sequential")]
    public class UpdateContractor: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateClientById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var contractorToCompare = await db._repository.Contractor.GetById(1);

                //ASSERT
                Assert.NotNull(contractorToCompare);

                if (contractorToCompare is not null)
                {
                    var updateContractor = new ContractorUpdateRequest
                    {
                        Owner = contractorToCompare.Owner,
                        AddressId = 2,
                        ContractorName = "TestName",
                        IN = 123456789,
                        TIN = "TestTIN",
                        Mobil = "+420774876504",
                        Tel = "+420774876504",
                        Email = "TestMail",
                        Www = "www.test.cz"
                    };

                    var updateContractorResult = await db._repository.Contractor.Update(contractorToCompare.Id, updateContractor, null);

                    await db._repository.Save();
                    Assert.True(updateContractorResult);

                    var updatedContractor = await db._context.Contractor.FindAsync(contractorToCompare.Id);
                    Assert.NotNull(updatedContractor);

                    if (updatedContractor is not null)
                    {
                        Assert.Equal(updatedContractor.AddressId, updateContractor.AddressId);
                        Assert.Equal(updatedContractor.ContractorName, updateContractor.ContractorName);
                        Assert.Equal(updatedContractor.IN, updateContractor.IN);
                        Assert.Equal(updatedContractor.TIN, updateContractor.TIN);
                        Assert.Equal(updatedContractor.Tel, updateContractor.Tel);
                        Assert.Equal(updatedContractor.Mobil, updateContractor.Mobil);
                        Assert.Equal(updatedContractor.Email, updateContractor.Email);
                        Assert.Equal(updatedContractor.Www, updateContractor.Www);
                    }
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}