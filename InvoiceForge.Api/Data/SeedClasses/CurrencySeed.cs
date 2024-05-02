using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class CurrencySeed
    {
        public List<Currency> Populate()
        {
            return new List<Currency>
            {
                new Currency()
                {
                    Value = "CZ"
                },
                new Currency()
                {
                    Value = "EUR"
                },

            };
        }
    }
}