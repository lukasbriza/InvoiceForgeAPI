using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.Data.Enum;

namespace InvoiceForgeApi.Model
{
    public class Numbering{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")] public int Owner { get; set; }
        [Required] public List<NumberingVariable> NumberingTemplate { get; set; } = new List<NumberingVariable>();
        public string? NumberingPrefix {get; set;} = null!;
        
        //Reference
        public virtual User User { get; set; } = null!;
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = null!;
    }
}