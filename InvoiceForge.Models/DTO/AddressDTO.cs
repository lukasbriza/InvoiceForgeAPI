using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.DTO.Model
{
    public class AddressGetRequest
    {
        public AddressGetRequest() {}
        public AddressGetRequest(Address? address, bool? plain = false)
        {
            if(address is not null)
            {
                Id = address.Id;
                Owner = address.Owner;
                Street = address.Street;
                StreetNumber = address.StreetNumber;
                City = address.City;
                PostalCode = address.PostalCode;
                CountryId = address.CountryId;
                Country = plain == false ? new CountryGetRequest(address.Country) : null;
            }
        }
        public int Id { get; set; }
        public int Owner {  get; set; }
        public string Street { get; set; } = null!;
        public int StreetNumber { get; set; }
        public string City { get; set; } = null!;
        public int PostalCode { get; set; }
        public int CountryId { get; set; }
        public CountryGetRequest? Country { get; set; } = null!;
    }
    public class AddressAddRequest
    {
        public int CountryId { get; set; }
        public string Street { get; set; } = null!;
        public int StreetNumber { get; set; }
        public string City { get; set; } = null!;
        public int PostalCode { get; set; }
    }

    public class AddressUpdateRequest
    {
        public int Owner {  get; set; }
        public int? CountryId { get; set; }
        public string? Street { get; set; }
        public int? StreetNumber { get; set; }
        public string? City { get; set; }
        public int? PostalCode { get; set; }
    }
}
