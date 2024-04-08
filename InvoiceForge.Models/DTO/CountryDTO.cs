using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.DTO.Model
{
    public class CountryGetRequest
    {
        public CountryGetRequest(){} 
        public CountryGetRequest(Country? country)
        {
            if (country is not null)
            {
                Id = country.Id;
                Value = country.Value;
                Shortcut = country.Shortcut;
            }
        }
        public int Id { get; set; }
        public string Value { get; set; } = null!;
        public string Shortcut { get; set; } = null!;
    }
}
