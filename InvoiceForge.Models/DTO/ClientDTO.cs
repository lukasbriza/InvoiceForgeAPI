using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Models
{
    public class ClientEntityBase
    {
        public int AddressId { get; set; }
        public string Name { get; set; } = null!;
        public long IN { get; set; }
        public string TIN { get; set; } = null!;
        public string? Mobil { get; set; } = null!;
        public string? Tel { get; set; } = null!;
        public string? Email { get; set; } = null!;
    }

    public class ClientGetRequest: ClientEntityBase
    {
        public ClientGetRequest(){}
        public ClientGetRequest(Client? client, bool? plain = false)
        {
            if (client is not null)
            {
                Id = client.Id;
                AddressId = client.AddressId;
                Owner = client.Owner;
                Type = client.Type;
                Name = client.Name;
                IN = client.IN;
                TIN = client.TIN;
                Mobil = client.Mobil;
                Tel = client.Tel;
                Email = client.Email;
                Address = plain == false ? new AddressGetRequest(client.Address, plain) : null;
            }
        }
        public int Id { get; set; }
        public int Owner { get; set; }
        public ClientType Type { get; set; }
        public AddressGetRequest? Address { get; set; } = null!;
    }

    public class ClientAddRequest: ClientEntityBase
    {
        public int TypeId { get; set; }
    }

    public class ClientUpdateRequest: ClientEntityBase
    {
        public int Owner { get; set; }
        public int TypeId { get; set; }
    }
}
