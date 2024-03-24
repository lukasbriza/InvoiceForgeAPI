using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class InvoiceServiceSeed
    {
        public List<InvoiceService> Populate()
        {
            return new List<InvoiceService>
            {
                new InvoiceService()
                {
                    InvoiceId = 1,
                    InvoiceItemId = 1,
                    Units = 2,
                    PricePerUnit = 500,
                    BasePrice = 1000,
                    VAT = 0,
                    Total = 1000
                },
                new InvoiceService()
                {
                    InvoiceId = 2,
                    InvoiceItemId = 2,
                    Units = 1,
                    PricePerUnit = 500,
                    BasePrice = 500,
                    VAT = 105,
                    Total = 605
                },
                new InvoiceService()
                {
                    InvoiceId = 2,
                    InvoiceItemId = 3,
                    Units = 1,
                    PricePerUnit = 500,
                    BasePrice = 500,
                    VAT = 105,
                    Total = 605
                }
            };
        }
    }
}