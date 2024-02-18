using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Address")] public int? AddressId { get; set; }
        [ForeignKey("User")] public int Owner { get; set; }

        [Required] public ClientType Type { get; set; }
        [Required] public string ClientName { get; set; } = null!;
        [Required] public long IN { get; set; }
        [Required] public string TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null;
        public string? Tel { get; set; } = null;
        public string? Email { get; set; } = null;

        //Reference
        public virtual Address? Address { get; set; }
        public virtual User User { get; set; } = null!;
    }
}