namespace InvoiceForgeApi.Configuration
{
    public static class ConnectionStringBuilder
    {
        public static string Build(IConfiguration config,  string env)
        {
            var host = config[$"{env}.Database:host"];
            var name = config[$"{env}.Database:databaseName"];
            var pwd = config[$"{env}.Database:password"];
            return $"Server={host}; Initial Catalog={name};User Id=sa;Password={pwd};Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Trusted_Connection=False;Application Intent=ReadWrite;Multi Subnet Failover=False;MultipleActiveResultSets=True";
        }
    }
}