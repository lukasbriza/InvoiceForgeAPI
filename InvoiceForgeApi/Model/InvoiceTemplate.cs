using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class InvoiceTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }
        [ForeignKey("User")] public int Owner { get; set; }
        
        [Required] public int ClientId { get; set; }
        [Required] public int ContractorId { get; set; }
        [Required] public int UserAccountId { get; set; }
        [Required] public string TemplateName { get; set; }
        [Required] public DateTime Created { get; set; }

        // reference
        public virtual User User { get; set; }
    }
}
