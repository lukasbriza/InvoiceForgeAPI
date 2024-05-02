using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using Xunit;

namespace Repository
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
                
                
                //ASSERT
                var tContractor = new TestContractor();
                var addContractor = new ContractorAddRequest
                {
                    AddressId = 1,
                    TypeId = 1,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Email = tContractor.Email,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Www = tContractor.Www
                };

                var addContractorResult = await db._repository.Contractor.Add(1, addContractor, ClientType.LegalEntity);
                Assert.NotNull(addContractorResult);

                if (addContractorResult is not null)
                {
                    Assert.IsType<int>(addContractorResult);
                    var newContractor = await db._context.Contractor.FindAsync(addContractorResult);

                    Assert.Equal(addContractor.AddressId, newContractor?.AddressId);
                    Assert.Equal(ClientType.LegalEntity, newContractor?.Type);
                    Assert.Equal(addContractor.Name, newContractor?.Name);
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