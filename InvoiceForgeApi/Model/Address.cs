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
        
        [Required] public int Owner {  get; set; }
        [Required] public string Street { get; set; }
        [Required] public int StreetNumber { get; set; }
        [Required] public string City { get; set; }
        [Required] public int PostalCode { get; set; }

        //reference
        public virtual Country Country { get; set; }
    }
}
