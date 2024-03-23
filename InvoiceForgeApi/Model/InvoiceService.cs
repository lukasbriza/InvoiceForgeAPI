using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class InvoiceService
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
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