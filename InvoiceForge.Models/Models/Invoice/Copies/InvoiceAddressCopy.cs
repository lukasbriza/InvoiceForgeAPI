using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.Models.CodeLists;
using InvoiceForgeApi.Models.DTO;

namespace InvoiceForgeApi.Models
{
    public class InvoiceAddressCopy: WithIdModel
    {
        public InvoiceAddressCopy() {}
        public InvoiceAddressCopy(int userId, InvoiceAddressCopyAddRequest address)
        {
            Owner = userId;
            Outdated = false;
            OriginId = address.OriginId;
            CountryId = address.CountryId;
            Street = address.Street;
            StreetNumber = address.StreetNumber;
            City = address.City;
            PostalCode = address.PostalCode;
        }
        [Required] public bool Outdated { get; set; } = false;
        [Required] public int Owner {  get; set; }
        [Required] public int OriginId {  get; set; }
        [ForeignKey("Country")] public int CountryId { get; set; }
        [Required] public string Street { get; set; } = null!;
        [Required] public int StreetNumber { get; set; }
        [Required] public string City { get; set; } = null!;
        [Required] public int PostalCode { get; set; }

        //Reference
        public virtual Country? Country { get; set; }
        public virtual InvoiceEntityCopy InvoiceEntityCopies { get; set; } = null!;
    }
}