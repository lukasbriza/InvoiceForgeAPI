using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class InvoiceItemSeed
    {
        public List<InvoiceItem> Populate()
        {
            return new List<InvoiceItem>
            {
                new InvoiceItem()
                {
                    Owner = 1,
                    ItemName = "ZeroTaxItemName",
                    TariffId = 1
                },
                new InvoiceItem()
                {
                    Owner = 1,
                    ItemName = "IT Service",
                    TariffId = 3
                },
                new InvoiceItem()
                {
                    Owner = 1,
                    ItemName = "Monitoring",
                    TariffId = 3
                },
                new InvoiceItem()
                {
                    Owner = 1,
                    ItemName = "Programming",
                    TariffId = 1
                }
            };
        }
    }
}