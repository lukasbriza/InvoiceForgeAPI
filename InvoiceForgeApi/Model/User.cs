using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceForgeApi.Model
{
    public class User
    {
        [Key] public int Id { get; set; }
        [Required] public int AuthenticationId { get; set; }
    }
}
