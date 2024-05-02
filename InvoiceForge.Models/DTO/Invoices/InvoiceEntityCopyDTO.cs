using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Models.DTO
{
    public class InvoiceEntityCopyGetRequest
    {
        public InvoiceEntityCopyGetRequest() {}
        public InvoiceEntityCopyGetRequest(InvoiceEntityCopy? invoiceEntity, bool? plain = false)
        {
            if (invoiceEntity is not null)
            {
                Id = invoiceEntity.Id;
                OriginClientId = invoiceEntity.OriginClientId;
                OriginContractorId = invoiceEntity.OriginContractorId;
                Owner = invoiceEntity.Owner;
                AddressCopyId = invoiceEntity.AddressCopyId;
                Type = invoiceEntity.Type;
                Name = invoiceEntity.Name;
                IN = invoiceEntity.IN;
                TIN = invoiceEntity.TIN;
                Mobil = invoiceEntity?.Mobil;
                Tel = invoiceEntity?.Tel;
                Email = invoiceEntity?.Email;
                Www = invoiceEntity?.Www;
                AddressCopy = plain == false ? new InvoiceAddressCopyGetRequest(invoiceEntity?.AddressCopy, plain) : null;
            }
        }
        public int Id { get; set; }
        public int? OriginClientId {  get; set; } = null;
        public int? OriginContractorId { get; set; } = null;
        public int Owner { get; set; }
        public int AddressCopyId { get; set; }
        public ClientType Type { get; set; }
        public string Name { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Www { get; set; }
        public InvoiceAddressCopyGetRequest? AddressCopy { get; set;}
    }

    public class InvoiceEntityCopyAddRequest
    {
        public InvoiceEntityCopyAddRequest() {}
        public InvoiceEntityCopyAddRequest(ClientGetRequest? client, int? copyAddressId)
        {
            if (client is not null && copyAddressId is not null)
            {
                Owner = client.Owner;
                OriginClientId = client.Id;
                OriginContractorId = null;
                AddressCopyId = (int)copyAddressId;
                Type = client.Type;
                Name = client.Name;
                IN = client.IN;
                TIN = client.TIN;
                Mobil = client?.Mobil;
                Tel = client?.Tel;
                Email = client?.Email;
                Www = null;
            }
        }
        public InvoiceEntityCopyAddRequest(ContractorGetRequest? contractor, int? copyAddressId)
        {
            if (contractor is not null && copyAddressId is not null)
            {
                Owner = contractor.Owner;
                OriginContractorId = contractor.Id;
                OriginClientId = null;
                AddressCopyId = (int)copyAddressId;
                Type = contractor.Type;
                Name = contractor.Name;
                IN = contractor.IN;
                TIN = contractor.TIN;
                Mobil = contractor?.Mobil;
                Tel = contractor?.Tel;
                Email = contractor?.Email;
                Www = contractor?.Www;
            }
        }

        public int Owner { get; set; }
        public int? OriginClientId {  get; set; } = null;
        public int? OriginContractorId { get; set; } = null;
        public int AddressCopyId { get; set; }
        public ClientType Type { get; set; }
        public string Name { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Www { get; set; }
    }

}