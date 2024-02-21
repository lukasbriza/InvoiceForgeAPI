﻿using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.DTO.Model
{
    public class ContractorGetRequest
    {
        public int Id { get; set; }
        public int Owner { get; set; }
        public ClientType ClientType { get; set; }
        public string ContractorName { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? www { get; set; }
        public AddressGetRequest Address { get; set; } = null!;
    }
}