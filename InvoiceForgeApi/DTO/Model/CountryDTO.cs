namespace InvoiceForgeApi.DTO.Model
{
    public class CountryGetRequest
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;
        public string Shortcut { get; set; } = null!;
    }
}
