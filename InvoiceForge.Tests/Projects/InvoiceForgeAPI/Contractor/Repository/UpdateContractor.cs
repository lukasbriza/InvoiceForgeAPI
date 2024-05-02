using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using Xunit;

namespace Repository
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
                
                var contractorToCompare = await db._repository.Contractor.GetById(1);

                //ASSERT
                Assert.NotNull(contractorToCompare);

                if (contractorToCompare is not null)
                {
                    var tContractor = new TestContractor();
                    var updateContractor = new ContractorUpdateRequest
                    {
                        Owner = contractorToCompare.Owner,
                        AddressId = 2,
                        Name = tContractor.Name,
                        IN = tContractor.IN,
                        TIN = tContractor.TIN,
                        Mobil = tContractor.Mobil,
                        Tel = tContractor.Tel,
                        Email = tContractor.Email,
                        Www = tContractor.Email
                    };

                    var updateContractorResult = await db._repository.Contractor.Update(contractorToCompare.Id, updateContractor, tContractor.Type);

                    await db._repository.Save();
                    Assert.True(updateContractorResult);

                    var updatedContractor = await db._context.Contractor.FindAsync(contractorToCompare.Id);
                    Assert.NotNull(updatedContractor);

                    if (updatedContractor is not null)
                    {
                        Assert.Equal(updatedContractor.AddressId, updateContractor.AddressId);
                        Assert.Equal(updatedContractor.Name, updateContractor.Name);
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

        [Fact]
        public Task ThrowErrorWhenUpdateIdenticValues()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var contractor = await db._context.Contractor.FindAsync(1);

                //ASSERT
                Assert.NotNull(contractor);
                var updateContractor = new ContractorUpdateRequest
                {
                    Owner = contractor!.Owner,
                    AddressId = contractor.AddressId,
                    Name = contractor.Name,
                    IN = contractor.IN,
                    TIN = contractor.TIN,
                    Email = contractor.Email,
                    Mobil = contractor.Mobil,
                    Tel = contractor.Tel,
                    Www = contractor.Www
                };

                try
                {
                    var result = await db._repository.Contractor.Update(1, updateContractor, contractor.Type);
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