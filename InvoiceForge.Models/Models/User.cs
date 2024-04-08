using System.ComponentModel.DataAnnotations;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Model
{
    public class User: WithIdModel
    {
        public User() {}
        public User(UserAddRequest user)
        {

        }
        [Required] public int AuthenticationId { get; set; }

        //Reference
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
        public virtual ICollection<Contractor> Contractors { get; set; } = new List<Contractor>();
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual ICollection<InvoiceTemplate> InvoiceTemplates { get; set; } = new List<InvoiceTemplate>();
        public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        public virtual ICollection<Numbering> Numberings { get; set;} = new List<Numbering>();
    }
}
