using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model.CodeLists
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Value { get; set; } = null!;
        [Required] public string Shortcut { get; set; } = null!;

        //Reference
        public virtual ICollection<Address>? Addresses { get; set; } = new List<Address>();
    }
}
