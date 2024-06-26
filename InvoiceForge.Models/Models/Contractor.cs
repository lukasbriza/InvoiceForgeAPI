﻿using InvoiceForgeApi.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Models
{
    public class Contractor: ModelBase
    {
        public Contractor() {}
        public Contractor(int userId, ContractorAddRequest contractor, ClientType clientType)
        {
            AddressId = contractor.AddressId;
            Owner = userId;
            Type = clientType;
            Name = contractor.Name;
            IN = contractor.IN;
            TIN = contractor.TIN;
            Email = contractor.Email;
            Mobil = contractor.Mobil;
            Tel = contractor.Tel;
            Www = contractor.Www;
        }
        [ForeignKey("Address")] public int AddressId { get; set; }
        [Required] public ClientType Type { get; set; }
        [Required] public string Name { get; set; } = null!;
        [Required] public long IN { get; set; }
        [Required] public string TIN { get; set; } = null!;
        [Required] public string Email { get; set; } = null!;
        public string? Mobil { get; set; } = null;
        public string? Tel { get; set; } = null;
        public string? Www { get; set; } = null;

        //Reference
        public virtual User User { get; set; } = null!; 
        public virtual Address? Address { get; set; }
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; } = new List<InvoiceTemplate>();
    }
}
