using FunctionalTests.Projects.InvoiceForgeApi;
using Xunit;

namespace FunctionalTests.Projects.InvoiceForgeAPI.Contractor.Repository
{
    [Collection("Sequential")]
    public class AddContractor: WebApplicationFactory
    {
        [Fact]
        public Task CanAddContractor()
        {
            return RunTest(async (client) => {
                //SETUP
                //ASSERT
                //CLEAN
            });
        }
    }
}