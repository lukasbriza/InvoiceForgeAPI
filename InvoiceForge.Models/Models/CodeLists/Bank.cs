using System.ComponentModel.DataAnnotations;

namespace InvoiceForgeApi.Model.CodeLists
{
    public class Bank: CodeListBase
    {
        [Required] public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; } = null;

        //Reference
        public virtual ICollection<UserAccount>? UserAccounts { get; set; } = new List<UserAccount>();
    }
}
