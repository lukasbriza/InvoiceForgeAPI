using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Models
{
    public class ModelBase: WithIdModel, IEntityBase
    {
        [ForeignKey("User")] public int Owner {  get; set; }
    }

    public class WithIdModel: IEntityId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }

    public class CodeListBase: ICodeListBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Value { get; set; } = null!;
    }
}