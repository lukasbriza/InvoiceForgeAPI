using System.ComponentModel.DataAnnotations;

namespace InvoiceForgeApi.Models.CodeLists
{
    public class Bank: CodeListBase
    {
        [Required] public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; } = null;

        //Reference
        public virtual ICollection<UserAccount>? UserAccounts { get; set; } = new List<UserAccount>();
        public virtual ICollection<InvoiceUserAccountCopy>? UserAccountCopies { get; set; } = new List<InvoiceUserAccountCopy>();
    }
}
