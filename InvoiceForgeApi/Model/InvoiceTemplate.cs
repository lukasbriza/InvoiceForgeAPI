using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Model
{
    public class InvoiceTemplate: ModelBase
    {
        [ForeignKey("Currency")] public int CurrencyId { get; set; } 
        [ForeignKey("Numbering")] public int NumberingId { get; set; }
        [ForeignKey("Client")] public int ClientId { get; set; }
        [ForeignKey("Contractor")] public int ContractorId { get; set; }
        [ForeignKey("UserAccount")] public int UserAccountId { get; set; }
        [Required] public string TemplateName { get; set; } = null!;
        [Required] public DateTime Created { get; set; }

        // Reference
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual Currency? Currency { get; set; }

        public virtual Client? Client { get; set; }
        public virtual Contractor? Contractor { get; set; }
        public virtual UserAccount? UserAccount { get; set; }
        public virtual Numbering? Numbering { get; set; }
    }
}
