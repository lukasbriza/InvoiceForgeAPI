using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class ContractorSeed
    {
        public List<Contractor> Populate()
        {
            return new List<Contractor>() {
                new Contractor()
                {
                    AddressId= 1,
                    Owner = 1,
                    Type = ClientType.Entrepreneur,
                    Name = "Contractor1",
                    IN = 123456789,
                    TIN = "CZ123456789",
                    Email = "email@seznam.cz",
                    Www = "www.web.cz",
                    Tel = "774876504",
                    Mobil = "774876504"
                }
            };
        }
    }
}
