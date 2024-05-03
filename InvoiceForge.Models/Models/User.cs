using System.ComponentModel.DataAnnotations;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Models
{
    public class User: WithIdModel, ITrackable
    {
        public User() {}
        public User(UserAddRequest user)
        {
            if (user is not null)
            {
                AuthenticationId = user.AuthenticationId;
            }
        }
        [Required] public int AuthenticationId { get; set; }
        [Required] public DateTime Created { get; set; }
        public DateTime? Updated { get; set; } = null;

        //Reference
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
        public virtual ICollection<Contractor> Contractors { get; set; } = new List<Contractor>();
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual ICollection<InvoiceTemplate> InvoiceTemplates { get; set; } = new List<InvoiceTemplate>();
        public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        public virtual ICollection<Numbering> Numberings { get; set;} = new List<Numbering>();
        public virtual ICollection<InvoiceEntityCopy> InvoiceEntityCopies { get; set; } = new List<InvoiceEntityCopy>();
        public virtual ICollection<InvoiceAddressCopy> InvoiceAddressCopies { get; set;} = new List<InvoiceAddressCopy>();
    }
}
