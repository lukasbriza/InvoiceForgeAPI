using System.ComponentModel.DataAnnotations;
using InvoiceForgeApi.Data.Enum;

namespace InvoiceForgeApi.Model
{
    public class Numbering: ModelBase
    {

        [Required] public List<NumberingVariable> NumberingTemplate { get; set; } = new List<NumberingVariable>();
        public string? NumberingPrefix {get; set;} = null!;
        
        //Reference
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = null!;
    }
}