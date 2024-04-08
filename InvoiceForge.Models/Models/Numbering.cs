using System.ComponentModel.DataAnnotations;
using InvoiceForgeApi.Enum;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Model
{
    public class Numbering: ModelBase
    {
        public Numbering() {}
        public Numbering(int userId, NumberingAddRequest numbering)
        {
            Owner = userId;
            NumberingTemplate = numbering.NumberingTemplate;
            NumberingPrefix = numbering.NumberingPrefix ?? null;
        }
        [Required] public List<NumberingVariable> NumberingTemplate { get; set; } = new List<NumberingVariable>();
        public string? NumberingPrefix {get; set;} = null!;
        
        //Reference
        public virtual User User { get; set; } = null!; 
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = null!;
    }
}