using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class InvoiceUserAccountCopySeed
    {
        public List<InvoiceUserAccountCopy> Populate()
        {
            return new List<InvoiceUserAccountCopy>() {
                new InvoiceUserAccountCopy() {
                    Outdated = false,
                    Owner = 1,
                    OriginId = 1,
                    BankId = 1,
                    AccountNumber = "321321-456654456",
                    IBAN = "CZ45646546654645"
                }
            };
        }
    }
}