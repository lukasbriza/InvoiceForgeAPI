using InvoiceForgeApi.Model;
using System.ComponentModel.DataAnnotations;

namespace InvoiceForgeApi.DTO.Model
{
    public class UserAddRequest
    {
        [Required] public int AuthenticationId { get; set; }
    }

    public class UserUpdateRequest
    {
        [Required] public int Id { get; set; }
        public int AuthenticationId { get; set; }

    }
    public class UserGetRequest
    {
        [Required] public int Id { get; set; }
        public IEnumerable<ClientGetRequest>? Clients { get; set; } = new List<ClientGetRequest>();
        public IEnumerable<ContractorGetRequest>? Contractors { get; set; } = new List<ContractorGetRequest>();
        public IEnumerable<UserAccountGetRequest>? UserAccounts { get; set; } = new List<UserAccountGetRequest>();
        public IEnumerable<InvoiceTemplateGetRequest>? InvoiceTemplates { get; set; } = new List<InvoiceTemplateGetRequest>();
        public IEnumerable<AddressGetRequest>? Addresses { get; set; } = new List<AddressGetRequest>();
        public IEnumerable<InvoiceItemGetRequest>? InvoiceItems { get; set; } = new List<InvoiceItemGetRequest>();
    }
}
