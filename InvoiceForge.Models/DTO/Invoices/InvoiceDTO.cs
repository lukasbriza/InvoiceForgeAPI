namespace InvoiceForgeApi.Models.DTO
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

                ClientCopyId = invoice.ClientCopyId;
                ContractorCopyId = invoice.ContractorCopyId;
                UserAccountCopyId = invoice.UserAccountCopyId;
                
                if (plain == false)
                {
                    var clientCopyFind = invoice.InvoiceEntityCopies.ToList().Find(c => c.Id == ClientCopyId);
                    var contractorCopyFind = invoice.InvoiceEntityCopies.ToList().Find(c => c.Id == ContractorCopyId);
                    
                    ClientCopy = new InvoiceEntityCopyGetRequest(clientCopyFind, plain);
                    ContractorCopy = new InvoiceEntityCopyGetRequest(contractorCopyFind, plain);
                } else {
                    ClientCopy = null;
                    ContractorCopy = null;
                }
                
                UserAccountCopy = plain == false ? new InvoiceUserAccountCopyGetRequest(invoice.InvoiceUserAccountCopy, plain) : null;
            }
        }
        public int Id { get; set; }
        public bool Outdated { get; set;}
        public int Owner { get; set; }
        public int TemplateId { get; set; }
        public int NumberingId { get; set; }
        public int ClientCopyId { get; set;}
        public int ContractorCopyId { get; set; }
        public int UserAccountCopyId { get; set; }
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

        public InvoiceEntityCopyGetRequest? ClientCopy { get; set; } = null!;
        public InvoiceEntityCopyGetRequest? ContractorCopy { get; set; } = null!;
        public InvoiceUserAccountCopyGetRequest? UserAccountCopy { get; set; } = null!;
        public string Currency { get; set; } = null!;
    }

    public class InvoiceAddRequest: InvoiceAddRequestBase
    {
        public List<InvoiceServiceAddRequest> InvoiceServices { get; set; } = new List<InvoiceServiceAddRequest>();
    }
    public class InvoiceAddRequestBase {
        public int TemplateId { get; set; }
        public DateTime Maturity { get; set; }
        public DateTime Exposure { get; set; }
        public DateTime TaxableTransaction { get; set; }
    }
    public class InvoiceAddRequestRepository: InvoiceAddRequestBase
    {
        public int ClientCopyId { get; set;}
        public int ContractorCopyId { get; set; }
        public int UserAccountCopyId { get; set; }
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
        public DateTime Maturity { get; set; }
        public DateTime Exposure { get; set; }
        public DateTime TaxableTransaction { get; set; }
    }
}