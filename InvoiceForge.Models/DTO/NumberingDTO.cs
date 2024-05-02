using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Models
{
    public class NumberingGetRequest
    {
        public NumberingGetRequest() {}
        public NumberingGetRequest(Numbering? numbering)
        {
            if (numbering is not null)
            {
                Id = numbering.Id;
                Owner = numbering.Owner;
                NumberingTemplate = numbering.NumberingTemplate;
                NumberingPrefix = numbering.NumberingPrefix;
            }
        }
        public int Id { get; set; }
        public int Owner { get; set; }
        public List<NumberingVariable> NumberingTemplate { get; set; } = new List<NumberingVariable>();
        public string? NumberingPrefix {get; set;} = null!;
    }
    public class NumberingUpdateRequest: NumberingAddRequest
    {
        public int Owner { get; set; }
    }
    public class NumberingAddRequest
    {
        public List<NumberingVariable> NumberingTemplate { get; set; } = new List<NumberingVariable>();
        public string? NumberingPrefix {get; set;} = null!;
    }

    public class GenerateInvoiceNumber
    {
        public string invoiceNumber { get; set; } = null!;
        public long invoiceOrder { get; set; }
    }
}