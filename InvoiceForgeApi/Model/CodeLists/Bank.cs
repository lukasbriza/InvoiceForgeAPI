using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model.CodeLists
{
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Value { get; set; } = null!;
        [Required] public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; } = null;

        //Reference
        public virtual ICollection<UserAccount>? UserAccounts { get; set; } = new List<UserAccount>();
    }
}
