using InvoiceForgeApi.Model.CodeLists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Country")] public int CountryId { get; set; }
        [ForeignKey("User")] public int Owner {  get; set; }
        [Required] public string Street { get; set; }
        [Required] public int StreetNumber { get; set; }
        [Required] public string City { get; set; }
        [Required] public int PostalCode { get; set; }

        //Reference
        public virtual Country? Country { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Client>? Clients { get; set; } = new List<Client>();
        public virtual ICollection<Contractor>? Contractors { get; set; } = new List<Contractor>();
    }
}
