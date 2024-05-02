using InvoiceForgeApi.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Models
{
    public class Client: ModelBase
    {
        public Client() {}
        public Client(int userId , ClientAddRequest client, ClientType clientType)
        {
            AddressId = client.AddressId;
            Owner = userId;
            Type = clientType;
            Name = client.Name;
            IN = client.IN;
            TIN = client.TIN;
            Mobil = client.Mobil;
            Tel = client.Tel;
            Email = client.Email;
        }
        [ForeignKey("Address")] public int AddressId { get; set; }

        [Required] public ClientType Type { get; set; }
        [Required] public string Name { get; set; } = null!;
        [Required] public long IN { get; set; }
        [Required] public string TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null;
        public string? Tel { get; set; } = null;
        public string? Email { get; set; } = null;

        //Reference
        public virtual User User { get; set; } = null!; 
        public virtual Address? Address { get; set; }
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = new List<InvoiceTemplate>();
    }
}