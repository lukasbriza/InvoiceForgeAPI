

using FunctionalTests.Projects.InvoiceForgeApi;
using Xunit;

namespace FunctionalTests.Projects.InvoiceForgeAPI.Contractor.Repository
{
    [Collection("Sequential")]
    public class UpdateContractor: WebApplicationFactory
    {
        public Task CanUpdateClientById()
        {
            return RunTest(async (client) => {
                //SETUP
                //ASSERT
                //CLEAN
            });
        }
    }
}