using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Models
{
    public class Address: ModelBase
    {
        public Address(){}
        public Address(int userId, AddressAddRequest address)
        {
            Owner = userId;
            CountryId = address.CountryId;
            Street = address.Street;
            StreetNumber = address.StreetNumber;
            City = address.City;
            PostalCode = address.PostalCode;
        }

        [ForeignKey("Country")] public int CountryId { get; set; }
        [Required] public string Street { get; set; } = null!;
        [Required] public int StreetNumber { get; set; }
        [Required] public string City { get; set; } = null!;
        [Required] public int PostalCode { get; set; }

        //Reference
        public virtual User User { get; set; } = null!; 
        public virtual Country? Country { get; set; }
        public virtual ICollection<Client>? Clients { get; set; } = new List<Client>();
        public virtual ICollection<Contractor>? Contractors { get; set; } = new List<Contractor>();
    }
}
