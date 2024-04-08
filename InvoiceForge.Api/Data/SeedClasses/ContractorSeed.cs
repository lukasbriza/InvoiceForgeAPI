using InvoiceForgeApi.Model;

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
                    ClientType = Enum.ClientType.Entrepreneur,
                    ContractorName = "Contractor1",
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
