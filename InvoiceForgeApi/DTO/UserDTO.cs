using InvoiceForgeApi.Model;
using System.ComponentModel.DataAnnotations;

namespace InvoiceForgeApi.DTO
{
    public class UserAddDTO
    {
        [Required] public int AuthenticationId { get; set; }
    }

    public class UserUpdateDTO: User { }
}
