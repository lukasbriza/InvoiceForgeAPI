using InvoiceForgeApi.Errors;
using Microsoft.Extensions.Configuration;

namespace InvoiceForge.Tests
{
    public static class GetConfiguration
    {
        private static readonly IConfiguration _configuration;
        static  GetConfiguration()
        {
            string[] dirtyPath = Directory.GetCurrentDirectory().Split("\\");
            string cleanedRootPath = "";
            bool cleaned = false;

            foreach (var part in dirtyPath){
                if (cleaned is false)
                {
                    if (part == "InvoiceForgeAPI") {
                        cleanedRootPath = cleanedRootPath.Length > 0 ? $"{cleanedRootPath}\\{part}" : $"{part}";
                        cleaned = true;
                        break;
                    }
                    cleanedRootPath = cleanedRootPath.Length > 0 ? $"{cleanedRootPath}\\{part}" : $"{part}";
                }
            }


            if (cleanedRootPath.Length > 0)
            {
                var configParent = Path.Combine(cleanedRootPath, "InvoiceForge.Api");
                var builder = new ConfigurationBuilder()
                    .SetBasePath(configParent)
                    .AddJsonFile("appsettings.json", true, true);

                _configuration = builder.Build();
                return;
            }

            throw new OperationError("Wrong appsettings parent directory.");
        }
        public static IConfiguration Get()
        {
            return _configuration;
        }
    }
}