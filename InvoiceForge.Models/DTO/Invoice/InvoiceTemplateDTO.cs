namespace InvoiceForgeApi.Models
{
    public class InvoiceTemplateEntityBase
    {
        public  int ClientId { get; set; }
        public int ContractorId { get; set; }
        public int UserAccountId { get; set; }
        public int CurrencyId { get; set; }
        public string TemplateName { get; set; } = null!;
        public int NumberingId { get; set; }
    }

    public class InvoiceTemplateGetRequest: InvoiceTemplateEntityBase
    {
        public InvoiceTemplateGetRequest() {}
        public InvoiceTemplateGetRequest(InvoiceTemplate? template, bool? plain = false) 
        {
            if (template is not null)
            {
                Id = template.Id;
                Owner = template.Owner;
                ClientId = template.ClientId;
                ContractorId = template.ContractorId;
                UserAccountId = template.UserAccountId;
                TemplateName = template.TemplateName;
                NumberingId = template.NumberingId;
                CurrencyId = template.CurrencyId;
                Created = template.Created;
                Numbering = plain == false ? new NumberingGetRequest(template.Numbering) : null;
            }
        }
        public int Id { get; set; }
        public int Owner { get; set; }
        public  NumberingGetRequest? Numbering { get; set; }
        public DateTime Created { get; set; }
    }

    public class InvoiceTemplateAddRequest: InvoiceTemplateEntityBase {}

    public class InvoiceTemplateUpdateRequest: InvoiceTemplateEntityBase
    {
        public int Owner { get; set; }
    }
}
