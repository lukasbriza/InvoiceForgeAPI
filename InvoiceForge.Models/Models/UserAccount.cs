
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model.CodeLists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class UserAccount: ModelBase
    {
        public UserAccount() {}
        public UserAccount(int userId, UserAccountAddRequest userAccount) 
        {
            Owner = userId;
            BankId = userAccount.BankId;
            AccountNumber = userAccount.AccountNumber;
            IBAN = userAccount.IBAN;
        }
        [ForeignKey("Bank")] public int? BankId { get; set; }

        [Required] public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; } = null;

        //Reference
        public virtual User User { get; set; } = null!; 
        public virtual Bank? Bank { get; set; }
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = new List<InvoiceTemplate>();

    }
}
