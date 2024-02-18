using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.DTO.Model
{
    public class AddressGetRequest
    {
        public int Id { get; set; }
        public int Owner {  get; set; }
        public string Street { get; set; } = null!;
        public int StreetNumber { get; set; }
        public string City { get; set; } = null!;
        public int PostalCode { get; set; }
        public CountryGetRequest Country { get; set; } = null!;
    }
}
