using InvoiceForgeApi.Model.CodeLists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class UserAccount
    {
        [Key] public int Id { get; set; }
        [Required] public int Owner { get; set; }
        [ForeignKey("Bank")] public int BankId { get; set; }
        [Required] public Bank Bank { get; set; }
        [Required] public string AccountNumber { get; set; }
        public string? IBAN { get; set; }
    }
}
