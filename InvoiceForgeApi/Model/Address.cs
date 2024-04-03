using InvoiceForgeApi.Model.CodeLists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class Address: ModelBase
    {
        [ForeignKey("Country")] public int CountryId { get; set; }
        [Required] public string Street { get; set; } = null!;
        [Required] public int StreetNumber { get; set; }
        [Required] public string City { get; set; } = null!;
        [Required] public int PostalCode { get; set; }

        //Reference
        public virtual Country? Country { get; set; }
        public virtual ICollection<Client>? Clients { get; set; } = new List<Client>();
        public virtual ICollection<Contractor>? Contractors { get; set; } = new List<Contractor>();
    }
}
