using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.DTO.Model
{
    public class InvoiceTemplateGetRequest
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
        public  int ClientId { get; set; }
        public int ContractorId { get; set; }
        public int UserAccountId { get; set; }
        public string TemplateName { get; set; } = null!;
        public int NumberingId { get; set; }
        public int CurrencyId { get; set; }
        public  NumberingGetRequest? Numbering { get; set; }
        public DateTime Created { get; set; }
    }

    public class InvoiceTemplateAddRequest
    {
        public  int ClientId { get; set; }
        public int ContractorId { get; set; }
        public int UserAccountId { get; set; }
        public int CurrencyId { get; set; }
        public string TemplateName { get; set; } = null!;
        public int NumberingId { get; set; }
    }

    public class InvoiceTemplateUpdateRequest
    {
        public int Owner { get; set; }
        public int? ClientId { get; set; }
        public int? ContractorId { get; set; }
        public int? UserAccountId { get; set; }
        public int? NumberingId { get; set; }
        public int? CurrencyId { get; set; } 
        public string? TemplateName { get; set; }
    }
}
