using System.ComponentModel.DataAnnotations;

namespace InvoiceForgeApi.Model.CodeLists
{
    public class Bank
    {
        [Key] public int Id { get; set; }
        [Required] public string Value { get; set; }
        [Required] public string Shortcut { get; set; }
        public string? SWIFT { get; set; }
    }
}
