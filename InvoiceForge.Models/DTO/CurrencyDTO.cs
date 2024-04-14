using InvoiceForgeApi.Models.CodeLists;

namespace InvoiceForgeApi.DTO.Model
{
    public class CurrencyGetRequest
    {
        public CurrencyGetRequest(){} 
        public CurrencyGetRequest(Currency? currency)
        {
            if (currency is not null)
            {
                Id = currency.Id;
                Value = currency.Value;
            }
        }
        public int Id { get; set; }
        public string Value { get; set; } = null!;
    }
}