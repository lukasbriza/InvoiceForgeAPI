using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Models
{
    public class Tariff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public int Value { get; set; }
        //Reference
        public virtual ICollection<InvoiceItem>? InvoiceItems { get; set; } = new List<InvoiceItem>();
    }

}