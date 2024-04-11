using Microsoft.AspNetCore.Mvc.Testing;


namespace FunctionalTests.Projects.InvoiceForgeApi
{
    public class WebApplicationFactory
    {
        public static WebApplicationFactory<InvoiceForgeApiProgram> Application => new WebApplicationFactory<InvoiceForgeApiProgram>();
        protected static async Task RunTest(Func<HttpClient, Task> test)
        {
            
            var client = Application.CreateClient(new WebApplicationFactoryClientOptions{ AllowAutoRedirect = false});
            await test(client);
        }
    }
}