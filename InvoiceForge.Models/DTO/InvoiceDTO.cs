using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.DTO.Model
{
    public class InvoiceGetRequest
    {
        public InvoiceGetRequest() {}
        public InvoiceGetRequest(Invoice? invoice, bool? plain = false)
        {
            if (invoice is not null)
            {
                Id = invoice.Id;
                Outdated = invoice.Outdated;
                Owner = invoice.Owner;
                TemplateId = invoice.TemplateId;
                NumberingId = invoice.NumberingId;
                InvoiceNumber = invoice.InvoiceNumber;
                OrderNumber = invoice.OrderNumber;
                BasePriceTotal = invoice.BasePriceTotal;
                VATTotal = invoice.VATTotal;
                TotalAll = invoice.TotalAll;
                Currency = invoice.Currency;
                InvoiceServices = plain == false ? invoice.InvoiceServices.Select(s => new InvoiceServiceGetRequest(s)).ToList() : null;

                Created = invoice.Created;
                Maturity = invoice.Maturity;
                Exposure = invoice.Exposure;
                TaxableTransaction = invoice.TaxableTransaction;

                ClientLocal = invoice.ClientLocal;
                ContractorLocal = invoice.ContractorLocal;
                UserAccountLocal = invoice.UserAccountLocal;
            }
        }
        public int Id { get; set; }
        public bool Outdated { get; set;}
        public int Owner { get; set; }
        public int TemplateId { get; set; }
        public int NumberingId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public long OrderNumber { get; set; }
        public long BasePriceTotal { get; set; }
        public long VATTotal { get; set; }
        public long TotalAll { get; set; }
        public ICollection<InvoiceServiceGetRequest>? InvoiceServices { get; set; } = new List<InvoiceServiceGetRequest>();

        public DateTime Created { get; set; }
        public DateTime Maturity { get; set; }
        public DateTime Exposure { get; set; }
        public DateTime TaxableTransaction { get; set; }

        public ClientGetRequest ClientLocal { get; set; } = null!;
        public ContractorGetRequest ContractorLocal { get; set; } = null!;
        public UserAccountGetRequest UserAccountLocal { get; set; } = null!;
        public string Currency { get; set; } = null!;
    }

    public class InvoiceAddRequest: InvoiceAddRequestBase
    {
        public List<InvoiceServiceAddRequest> InvoiceServices { get; set; } = new List<InvoiceServiceAddRequest>();
    }
    public class InvoiceAddRequestBase{
        public int TemplateId { get; set; }
        public DateTime Maturity { get; set; }
        public DateTime Exposure { get; set; }
        public DateTime TaxableTransaction { get; set; }
    }
    public class InvoiceAddRequestRepository: InvoiceAddRequestBase
    {
        public ClientGetRequest ClientLocal { get; set;} = null!;
        public ContractorGetRequest ContractorLocal { get; set; } = null!;
        public UserAccountGetRequest UserAccountLocal { get; set; } = null!;
        public int NumberingId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public long OrderNumber { get; set; }
        public long BasePriceTotal { get; set; }
        public long VATTotal { get; set; }
        public long TotalAll { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime Created { get; set; }
    }
    public class InvoiceUpdateRequest
    {
        public int Owner { get; set; }
        public DateTime? Maturity { get; set; }
        public DateTime? Exposure { get; set; }
        public DateTime? TaxableTransaction { get; set; }
    }
}