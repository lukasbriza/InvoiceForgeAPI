using InvoiceForgeApi.Data.Enum;

namespace InvoiceForgeApi.DTO.Model
{

    public class ClientGetRequest
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int Owner { get; set; }
        public ClientType Type { get; set; }
        public string ClientName { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null!;
        public string? Tel { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public AddressGetRequest Address { get; set; } = null!;
    }

    public class ClientAddRequest
    {
        public int? AddressId { get; set; }
        public ClientType Type { get; set; }
        public string ClientName { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null!;
        public string? Tel { get; set; } = null!;
        public string? Email { get; set; } = null!;
    }

    public class ClientUpdateRequest
    {
        public int? AddressId { get; set; }
        public ClientType? Type { get; set; }
        public string? ClientName { get; set; } = null!;
        public long? IN { get; set; }
        public string? TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null!;
        public string? Tel { get; set; } = null!;
        public string? Email { get; set; } = null!;
    }
}
