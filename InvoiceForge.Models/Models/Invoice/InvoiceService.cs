using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Models
{
    public class InvoiceService: WithIdModel
    {
        public InvoiceService() {}
        public InvoiceService(int invoiceId, InvoiceServiceExtendedAddRequest invoiceService)
        {
            InvoiceId = invoiceId;
            InvoiceItemId = invoiceService.ItemId;
            Units = invoiceService.Units;
            PricePerUnit = invoiceService.PricePerUnit;
            BasePrice = invoiceService.BasePrice;
            VAT = invoiceService.VAT;
            Total = invoiceService.Total;
        }

        [ForeignKey("Invoice")] public int InvoiceId { get; set; }
        [ForeignKey("InvoiceItem")] public int InvoiceItemId { get; set; }
        [Required] public long Units { get; set; }
        [Required] public long PricePerUnit { get; set; }
        [Required] public long BasePrice { get; set; }
        [Required] public long VAT { get; set; }
        [Required] public long Total { get; set; }

        //Reference
        public virtual Invoice? Invoice { get; set; } = null!;
        public virtual InvoiceItem? InvoiceItem { get; set; } = null!;
    }
}