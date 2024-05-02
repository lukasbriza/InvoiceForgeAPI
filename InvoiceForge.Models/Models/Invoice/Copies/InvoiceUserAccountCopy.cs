using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.Models.CodeLists;
using InvoiceForgeApi.Models.DTO;

namespace InvoiceForgeApi.Models
{
    public class InvoiceUserAccountCopy: WithIdModel
    {
        public InvoiceUserAccountCopy() {}

        public InvoiceUserAccountCopy(int userId, InvoiceUserAccountCopyAddRequest userAccountCopy)
        {
            Owner = userId;
            Outdated = false;
            OriginId = userAccountCopy.OriginId;
            BankId = userAccountCopy.BankId;
            AccountNumber = userAccountCopy.AccountNumber;
            IBAN = userAccountCopy?.IBAN;
        }

        [Required] public bool Outdated { get; set; } = false;
        [Required] public int Owner {  get; set; }
        [Required] public int OriginId {  get; set; }
        [ForeignKey("Bank")] public int BankId { get; set; }
        [Required] public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; } = null;
        
        //Reference
        public virtual Bank? Bank { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set;} = new List<Invoice>();
    }
}