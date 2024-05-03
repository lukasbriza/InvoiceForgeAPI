using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Models
{
    public class InvoiceTemplate: ModelBase
    {
        public InvoiceTemplate() {}
        public InvoiceTemplate(int userId, InvoiceTemplateAddRequest template)
        {
            Owner = userId;
            ClientId = template.ClientId;
            ContractorId = template.ContractorId;
            UserAccountId = template.UserAccountId;
            CurrencyId = template.CurrencyId;
            TemplateName = template.TemplateName;
            NumberingId = template.NumberingId;
        }

        [ForeignKey("Currency")] public int CurrencyId { get; set; } 
        [ForeignKey("Numbering")] public int NumberingId { get; set; }
        [ForeignKey("Client")] public int ClientId { get; set; }
        [ForeignKey("Contractor")] public int ContractorId { get; set; }
        [ForeignKey("UserAccount")] public int UserAccountId { get; set; }
        [Required] public string TemplateName { get; set; } = null!;

        // Reference
        public virtual User User { get; set; } = null!; 
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual Currency? Currency { get; set; }

        public virtual Client? Client { get; set; }
        public virtual Contractor? Contractor { get; set; }
        public virtual UserAccount? UserAccount { get; set; }
        public virtual Numbering? Numbering { get; set; }
    }
}
