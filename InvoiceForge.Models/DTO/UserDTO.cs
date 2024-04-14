using System.ComponentModel.DataAnnotations;
using InvoiceForgeApi.Models;

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
        public UserGetRequest(){}
        public UserGetRequest(User? user, bool? plain = false)
        {
            if (user is not null)
            {
                Id = user.Id;
                Clients = plain == false ? user.Clients.Select(c => new ClientGetRequest(c)) : null;
                Contractors = plain == false ? user.Contractors.Select(c => new ContractorGetRequest(c)): null;
                UserAccounts = plain == false ?  user.UserAccounts.Select(u => new UserAccountGetRequest(u)) : null;
                Addresses = plain == false ? user.Addresses.Select(a => new AddressGetRequest(a)) : null;
                InvoiceItems = plain == false ? user.InvoiceItems.Select(i => new InvoiceItemGetRequest(i)) : null;
            }
        }
        [Required] public int Id { get; set; }
        public IEnumerable<ClientGetRequest>? Clients { get; set; } = new List<ClientGetRequest>();
        public IEnumerable<ContractorGetRequest>? Contractors { get; set; } = new List<ContractorGetRequest>();
        public IEnumerable<UserAccountGetRequest>? UserAccounts { get; set; } = new List<UserAccountGetRequest>();
        public IEnumerable<InvoiceTemplateGetRequest>? InvoiceTemplates { get; set; } = new List<InvoiceTemplateGetRequest>();
        public IEnumerable<AddressGetRequest>? Addresses { get; set; } = new List<AddressGetRequest>();
        public IEnumerable<InvoiceItemGetRequest>? InvoiceItems { get; set; } = new List<InvoiceItemGetRequest>();
    }
}
