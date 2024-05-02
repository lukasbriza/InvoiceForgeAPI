namespace InvoiceForgeApi.Models
{
    public class InvoiceServiceEntityBase
    {
        public long Units { get; set; }
        public long PricePerUnit { get; set; }
        public int ItemId { get; set; }
    }

    public class InvoiceServiceGetRequest: InvoiceServiceEntityBase
    {
        public InvoiceServiceGetRequest() {}
        public InvoiceServiceGetRequest(InvoiceService? invoiceService, bool? plain = false)
        {
            if (invoiceService is not null)
            {
                Id = invoiceService.Id;
                InvoiceId = invoiceService.InvoiceId;
                ItemId = invoiceService.InvoiceItemId;
                Units = invoiceService.Units;
                PricePerUnit = invoiceService.PricePerUnit;
                BasePrice = invoiceService.BasePrice;
                VAT = invoiceService.VAT;
                Total = invoiceService.Total;
                Item = plain == false ? new InvoiceItemGetRequest(invoiceService.InvoiceItem) : null;
            }
        }
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public long BasePrice { get; set; }
        public long VAT { get; set; }
        public long Total { get; set; }
        public InvoiceItemGetRequest? Item { get; set; }
    }

    public class InvoiceServiceAddRequest: InvoiceServiceEntityBase {}

    public class InvoiceServiceExtendedAddRequest: InvoiceServiceEntityBase
    {
        public long BasePrice { get; set; }
        public long VAT { get; set; }
        public long Total { get; set; }
    }
     public class InvoiceServiceUpdateRequest: InvoiceServiceEntityBase
    {
        public long BasePrice { get; set; }
    }
}