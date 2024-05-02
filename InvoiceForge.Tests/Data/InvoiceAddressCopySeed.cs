using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class InvoiceAddressCopySeed
    {
        public List<InvoiceAddressCopy> Populate()
        {
            return new List<InvoiceAddressCopy>() {
                new InvoiceAddressCopy() {
                    CountryId = 1,
                    OriginId = 1,
                    Owner = 1,
                    Street = "Street1",
                    StreetNumber = 120,
                    City = "Kolín",
                    PostalCode = 28002,
                },
                new InvoiceAddressCopy() {
                    CountryId = 2,
                    OriginId = 2,
                    Owner = 1,
                    Street = "Street2",
                    StreetNumber = 111,
                    City = "Praha",
                    PostalCode = 18000,
                }
            };
        }
    }
}