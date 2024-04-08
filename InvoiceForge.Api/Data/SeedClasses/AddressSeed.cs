using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class AddressSeed
    {
        public List<Address> Populate()
        {
            return new List<Address>()
            {
                new Address()
                {
                    CountryId = 1,
                    Owner = 1,
                    Street = "Street1",
                    StreetNumber = 120,
                    City = "Kolín",
                    PostalCode = 28002,
                },
                new Address()
                {
                    CountryId = 2,
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
