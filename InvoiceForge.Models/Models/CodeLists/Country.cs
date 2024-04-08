using System.ComponentModel.DataAnnotations;

namespace InvoiceForgeApi.Model.CodeLists
{
    public class Country: CodeListBase
    {
        [Required] public string Shortcut { get; set; } = null!;

        //Reference
        public virtual ICollection<Address>? Addresses { get; set; } = new List<Address>();
    }
}
