using InvoiceForgeApi.Enum;

namespace InvoiceForgeApi.DTO.Model
{

    public class ClientGetRequest
    {
        public ClientGetRequest(){}
        public ClientGetRequest(Client? client, bool? plain = false)
        {
            if (client is not null)
            {
                Id = client.Id;
                AddressId = (int)client.AddressId!;
                Owner = client.Owner;
                Type = client.Type;
                ClientName = client.ClientName;
                IN = client.IN;
                TIN = client.TIN;
                Mobil = client.Mobil;
                Tel = client.Tel;
                Email = client.Email;
                Address = plain == false ? new AddressGetRequest(client.Address, plain) : null;
            }
        }
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
        public AddressGetRequest? Address { get; set; } = null!;
    }

    public class ClientAddRequest
    {
        public int AddressId { get; set; }
        public int TypeId { get; set; }
        public string ClientName { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null!;
        public string? Tel { get; set; } = null!;
        public string? Email { get; set; } = null!;
    }

    public class ClientUpdateRequest
    {
        public int Owner { get; set; }
        public int? AddressId { get; set; }
        public int? TypeId { get; set; }
        public string? ClientName { get; set; } = null!;
        public long? IN { get; set; }
        public string? TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null!;
        public string? Tel { get; set; } = null!;
        public string? Email { get; set; } = null!;
    }
}
