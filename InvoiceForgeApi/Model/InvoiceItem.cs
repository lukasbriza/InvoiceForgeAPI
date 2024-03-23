using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Model
{
    public class InvoiceItem
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [ForeignKey("User")] public int Owner { get; set; }
        [Required] public string ItemName { get; set; } = null!;
        [ForeignKey("Tariff")] public int TariffId { get; set; } 
       
        //Reference
        public virtual User User { get; set; } = null!;
        public virtual Tariff? Tariff { get; set; }
        public virtual ICollection<InvoiceService>? InvoiceServices { get; set; } = new List<InvoiceService>();
    }
}