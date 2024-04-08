using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class TariffSeed
    {
        public List<Tariff> Populate()
        {
            return new List<Tariff>
            {
                new Tariff()
                {
                    Value = 0,
                },
                new Tariff()
                {
                    Value = 12,
                },
                new Tariff()
                {
                    Value = 21,
                }
            };
        }
    }
}