namespace InvoiceForgeApi.Models.CodeLists
{
    public class Currency: CodeListBase
    {
        //Reference
        public virtual ICollection<InvoiceTemplate>? InvoiceTemplates { get; set; }
    }

}