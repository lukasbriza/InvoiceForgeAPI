using System.ComponentModel.DataAnnotations;

namespace InvoiceForgeApi.Model.CodeLists
{
    public class Country
    {
        [Key] public int Id { get; set; }
        [Required] public string Value { get; set; }
        [Required] public string Shortcut { get; set; }
    }
}
