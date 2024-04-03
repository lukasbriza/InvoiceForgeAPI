using InvoiceForgeApi.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class Contractor: ModelBase
    {
        [ForeignKey("Address")] public int? AddressId { get; set; }
        [Required] public ClientType ClientType { get; set; }
        [Required] public string ContractorName { get; set; } = null!;
        [Required] public long IN { get; set; }
        [Required] public string TIN { get; set; } = null!;
        [Required] public string Email { get; set; } = null!;
        public string? Mobil { get; set; } = null;
        public string? Tel { get; set; } = null;
        public string? Www { get; set; } = null;

        //Reference
        public virtual Address? Address { get; set; }
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = new List<InvoiceTemplate>();
    }
}
