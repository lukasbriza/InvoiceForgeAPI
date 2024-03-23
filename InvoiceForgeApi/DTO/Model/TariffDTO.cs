using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.DTO.Model
{
    public class TariffGetRequest
    {
        public TariffGetRequest() {}
        public TariffGetRequest(Tariff? tariff)
        {
            if (tariff is not null)
            {
                Id = tariff.Id;
                Value = tariff.Value;
            }
        }
        public int Id { get; set; }
        public int Value { get; set; }
    }
}
