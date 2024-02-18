namespace InvoiceForgeApi.DTO.Model
{
    public class InvoiceTemplateGetRequest
    {
        public int Id { get; set; }
        public int Owner { get; set; }
        public  int ClientId { get; set; }
        public int ContractorId { get; set; }
        public int UserAccountId { get; set; }
        public string TemplateName { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
