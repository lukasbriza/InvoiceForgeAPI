namespace InvoiceForgeApi.Models
{
    public class InvoiceAddressCopyEntityBase
    {
        public int OriginId {  get; set; }
        public int CountryId { get; set; }
        public string Street { get; set; } = null!;
        public int StreetNumber { get; set; }
        public string City { get; set; } = null!;
        public int PostalCode { get; set; }
    }

    public class InvoiceAddressCopyGetRequest: InvoiceAddressCopyEntityBase
    {
        public InvoiceAddressCopyGetRequest() {}
        public InvoiceAddressCopyGetRequest(InvoiceAddressCopy? addressCopy, bool? plain = false)
        {
            if (addressCopy is not null)
            {
                Id = addressCopy.Id;
                OriginId = addressCopy.OriginId;
                CountryId = addressCopy.CountryId;
                Street = addressCopy.Street;
                StreetNumber = addressCopy.StreetNumber;
                City = addressCopy.City;
                PostalCode = addressCopy.PostalCode;
                Country = plain == false ? new CountryGetRequest(addressCopy.Country) : null;
            }
        }

        public int Id { get; set; }
        public CountryGetRequest? Country {get; set;}
    }

    public class InvoiceAddressCopyAddRequest: InvoiceAddressCopyEntityBase {}
}