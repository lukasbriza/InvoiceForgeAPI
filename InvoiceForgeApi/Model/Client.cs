using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi
{
    public class Client
    {
        [Key] public int Id { get; set; }
        [ForeignKey("Address")] public int AddressId { get; set; }
        
        [Required] public int Owner { get; set; }
        [Required] public ClientType Type { get; set; }
        [Required] public string ClientName { get; set; }
        public Address Address { get; set; }
        [Required] public int IN { get; set; }
        [Required] public string TIN { get; set; }
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
    }
}