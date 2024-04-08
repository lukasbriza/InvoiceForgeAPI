using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class UserAccountSeed
    {
        public List<UserAccount> Populate()
        {
            return new List<UserAccount>()
            {
                new UserAccount()
                {
                    Owner = 1,
                    BankId = 1,
                    AccountNumber = "321321-456654456",
                    IBAN = "CZ45646546654645"
                }
            };
        }
    }
}
