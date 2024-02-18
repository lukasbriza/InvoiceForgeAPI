using InvoiceForgeApi.Model.CodeLists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")] public int Owner { get; set; }
        [ForeignKey("Bank")] public int? BankId { get; set; }

        [Required] public string AccountNumber { get; set; }
        public string? IBAN { get; set; } = null;

        //Reference
        public virtual Bank? Bank { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
