using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model.CodeLists
{
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Value { get; set; } = null!;

        //Reference
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; }
    }

}