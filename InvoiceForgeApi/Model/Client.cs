using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi
{
    public class Client: ModelBase
    {
        [ForeignKey("Address")] public int? AddressId { get; set; }

        [Required] public ClientType Type { get; set; }
        [Required] public string ClientName { get; set; } = null!;
        [Required] public long IN { get; set; }
        [Required] public string TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null;
        public string? Tel { get; set; } = null;
        public string? Email { get; set; } = null;

        //Reference
        public virtual Address? Address { get; set; }
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = new List<InvoiceTemplate>();
    }
}