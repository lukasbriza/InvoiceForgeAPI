using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class CountrySeed
    {
        public List<Country> Populate()
        {
            return new List<Country>()
            {
                new Country()
                {
                    Value = "Česká republika",
                    Shortcut = "CZE"
                },
                new Country()
                {
                    Value = "Slovenská republika",
                    Shortcut = "SK"
                },
                new Country()
                {
                    Value = "Spojené státy",
                    Shortcut = "US"
                }
            };
        }
    }
}
