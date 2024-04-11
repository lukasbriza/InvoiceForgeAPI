using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.CodeLists;

namespace InvoiceForgeApi.Models
{
    public class InvoiceItem: ModelBase
    {
        public InvoiceItem() {}
        public InvoiceItem(int userId, InvoiceItemAddRequest invoiceItem)
        {
            Owner = userId;
            ItemName = invoiceItem.ItemName;
            TariffId = invoiceItem.TariffId;
        }
        [Required] public string ItemName { get; set; } = null!;
        [ForeignKey("Tariff")] public int TariffId { get; set; } 
       
        //Reference
        public virtual User User { get; set; } = null!; 
        public virtual Tariff? Tariff { get; set; }
        public virtual ICollection<InvoiceService>? InvoiceServices { get; set; } = new List<InvoiceService>();
    }
}