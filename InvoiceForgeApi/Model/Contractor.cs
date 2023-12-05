using InvoiceForgeApi.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class Contractor
    {
        [Key] public int Id { get; set; }
        [ForeignKey("Address")] public int AddressId { get; set; }
        
        [Required] public int Owner { get; set; }
        public ClientType ClientType { get; set; }
        [Required] public string ContractorName { get; set; }
        public Address Address { get; set; }
        [Required] public int IN { get; set; }
        [Required] public string TIN { get; set; }
        public string Email { get; set; }
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? www { get; set; }
    }
}
