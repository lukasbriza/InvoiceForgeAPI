namespace InvoiceForgeApi.Models.DTO
{
    public class InvoiceItemGetRequest
    {
        public InvoiceItemGetRequest() {}
        public InvoiceItemGetRequest(InvoiceItem? invoiceItem, bool? plain = false)
        {
            if(invoiceItem is not null)
            {
                Id = invoiceItem.Id;
                Owner = invoiceItem.Owner;
                ItemName = invoiceItem.ItemName;
                TariffId = invoiceItem.TariffId;
                Tariff = plain == false ? new TariffGetRequest(invoiceItem.Tariff) : null;
            }
        }
        public int Id { get; set; }
        public int Owner { get; set; }
        public string ItemName { get; set; } = null!;
        public int TariffId { get; set; }
        public TariffGetRequest? Tariff {get; set; } = null!;
    }

    public class InvoiceItemAddRequest
    {
        public string ItemName { get; set; } = null!;
        public int TariffId { get; set; }
    }
    public class InvoiceItemUpdateRequest: InvoiceItemAddRequest
    {
        public int Owner { get; set; }
    }
}