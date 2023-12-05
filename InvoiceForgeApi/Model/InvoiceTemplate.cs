using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class InvoiceTemplate
    {
        [Key] public int Id { get; set; }
        [ForeignKey("Client")] public int ClientId { get; set; }
        [ForeignKey("Contractor")] public int ContractorId { get; set; }
        [ForeignKey("UserAccount")] public int UserAccountId { get; set; }
        
        [Required] public int Owner { get; set; }
        [Required] public string TemplateName { get; set; }
        [Required] public DateTime Created { get; set; }
        [Required] public Contractor Contractor { get; set; }
        [Required] public UserAccount UserAccount { get; set; }
        [Required] public Client Client { get; set; }
    }
}
