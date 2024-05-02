using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Models
{
    public class InvoiceEntityCopy: WithIdModel
    {
        public InvoiceEntityCopy() {}
        public InvoiceEntityCopy(int userId, InvoiceEntityCopyAddRequest invoiceEntity)
        {
            Owner = userId;
            Outdated = false;
            OriginClientId = invoiceEntity.OriginClientId ?? null;
            OriginContractorId = invoiceEntity.OriginContractorId ?? null;
            AddressCopyId = invoiceEntity.AddressCopyId;
            Type = invoiceEntity.Type;
            Name = invoiceEntity.Name;
            IN = invoiceEntity.IN;
            TIN = invoiceEntity.TIN;
            Mobil = invoiceEntity?.Mobil;
            Tel = invoiceEntity?.Tel;
            Email = invoiceEntity?.Email;
            Www = invoiceEntity?.Www;
        }

        [Required] public bool Outdated { get; set; } = false;
        [Required] public int Owner {  get; set; }
        public int? OriginClientId {  get; set; } = null;
        public int? OriginContractorId { get; set; } = null;
        [ForeignKey("InvoiceAddressCopy")] public int AddressCopyId { get; set; }
        [Required] public ClientType Type { get; set; }
        [Required] public string Name { get; set; } = null!;
        [Required] public long IN { get; set; }
        [Required] public string TIN { get; set; } = null!;
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Www { get; set; }

        //References
        public virtual InvoiceAddressCopy AddressCopy { get; set; } = null!;
        public virtual ICollection<Invoice> Invoices { get; set;} = new List<Invoice>();
    }
}