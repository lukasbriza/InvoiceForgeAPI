using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Models.DTO
{
    public class ContractorGetRequest
    {
        public ContractorGetRequest(){}
        public ContractorGetRequest(Contractor? contractor, bool? plain = false)
        {
            if (contractor is not null)
            {
                Id = contractor.Id;
                Owner = contractor.Owner;
                Type = contractor.Type;
                Name = contractor.Name;
                IN = contractor.IN;
                TIN = contractor.TIN;
                Email = contractor.Email;
                Mobil = contractor.Mobil;
                Tel = contractor.Tel;
                Www = contractor.Www;
                Address = plain == false ? new AddressGetRequest(contractor.Address) : null;
            }
        }
        public int Id { get; set; }
        public int Owner { get; set; }
        public ClientType Type { get; set; }
        public int AddressId { get; set; }
        public string Name { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? Www { get; set; }
        public AddressGetRequest? Address { get; set; } = null!;
    }
    public class ContractorAddRequest
    {
        public int AddressId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Mobil { get; set; }
        public string? Tel { get; set; }
        public string? Www { get; set; }
    }
    public class ContractorUpdateRequest: ContractorAddRequest
    {
        public int Owner { get; set; }
    }
}
