namespace InvoiceForgeApi.Models
{
    public class Currency: CodeListBase
    {
        //Reference
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; }
    }

}