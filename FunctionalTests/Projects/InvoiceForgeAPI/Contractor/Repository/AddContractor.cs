using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO.Model;
using Xunit;

namespace ContractorRepository
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
                db.InitializeDbForTest();
                
                //ASSERT
                var addContractor = new ContractorAddRequest
                {
                    AddressId = 1,
                    TypeId = 1,
                    ContractorName = "TestContractorName",
                    IN = 123456789,
                    TIN = "TestTIN",
                    Email = "TestEmail",
                    Mobil = "+420774876504",
                    Tel = "+420774876504",
                    Www = "www.test.cz"
                };

                var addContractorResult = await db._repository.Contractor.Add(1, addContractor, ClientType.LegalEntity);
                Assert.NotNull(addContractorResult);

                if (addContractorResult is not null)
                {
                    Assert.IsType<int>(addContractorResult);
                    var newContractor = await db._context.Contractor.FindAsync(addContractorResult);

                    Assert.Equal(addContractor.AddressId, newContractor?.AddressId);
                    Assert.Equal(ClientType.LegalEntity, newContractor?.ClientType);
                    Assert.Equal(addContractor.ContractorName, newContractor?.ContractorName);
                    Assert.Equal(addContractor.IN, newContractor?.IN);
                    Assert.Equal(addContractor.TIN, newContractor?.TIN);
                    Assert.Equal(addContractor.Email, newContractor?.Email);
                    Assert.Equal(addContractor.Mobil, newContractor?.Mobil);
                    Assert.Equal(addContractor.Tel, newContractor?.Tel);
                    Assert.Equal(addContractor.Www, newContractor?.Www);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}