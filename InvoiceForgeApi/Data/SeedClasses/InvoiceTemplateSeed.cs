using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class InvoiceTemplateSeed
    {
        public List<InvoiceTemplate> Populate()
        {
            return new List<InvoiceTemplate>
            {
                new InvoiceTemplate()
                {
                    Owner = 1,
                    ClientId = 1,
                    ContractorId = 1,
                    UserAccountId = 1,
                    TemplateName = "Template1",
                    Created = DateTime.UtcNow,
                    NumberingId = 1,
                    CurrencyId = 1
                },
                new InvoiceTemplate()
                {
                    Owner = 1,
                    ClientId = 2,
                    ContractorId = 1,
                    UserAccountId = 1,
                    TemplateName = "Template2",
                    Created = DateTime.UtcNow,
                    NumberingId = 1,
                    CurrencyId = 2
                }
            };
        }
    }
}
