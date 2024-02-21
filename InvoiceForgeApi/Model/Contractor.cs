using InvoiceForgeApi.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class Contractor
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [ForeignKey("Address")] public int? AddressId { get; set; }
        [ForeignKey("User")] public int Owner { get; set; }

        [Required] public ClientType ClientType { get; set; }
        [Required] public string ContractorName { get; set; }
        [Required] public long IN { get; set; }
        [Required] public string TIN { get; set; }
        [Required] public string Email { get; set; } 
        public string? Mobil { get; set; } = null;
        public string? Tel { get; set; } = null;
        public string? www { get; set; } = null;

        //Reference
        public virtual Address? Address { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
