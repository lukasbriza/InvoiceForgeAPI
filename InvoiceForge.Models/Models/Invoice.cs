using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Model
{
    public class Invoice: ModelBase
    {
        public Invoice() {}
        public Invoice(int userId, InvoiceAddRequestRepository invoice) 
        {
                Outdated = false;
                Owner = userId;
                TemplateId = invoice.TemplateId;
                NumberingId = invoice.NumberingId;
                InvoiceNumber = invoice.InvoiceNumber;
                OrderNumber = invoice.OrderNumber;
                BasePriceTotal = invoice.BasePriceTotal;
                VATTotal = invoice.VATTotal;
                TotalAll = invoice.TotalAll;
                Currency = invoice.Currency;

                ClientLocal = invoice.ClientLocal;
                ContractorLocal = invoice.ContractorLocal;
                UserAccountLocal = invoice.UserAccountLocal;

                Maturity = invoice.Maturity;
                Exposure = invoice.Exposure;
                TaxableTransaction = invoice.TaxableTransaction;
                Created = invoice.Created;
        }
        public bool Outdated { get; set;} = false;
        [ForeignKey("InvoiceTemplate")] public int TemplateId { get; set; }
        [Required] public int NumberingId { get; set; }
        [Required] public string InvoiceNumber { get; set; } = null!;
        [Required] public long OrderNumber { get; set; }
        [Required] public long BasePriceTotal { get; set; }
        [Required] public long VATTotal { get; set; }
        [Required] public long TotalAll { get; set; }
        [Required] public string Currency { get; set; } = null!;

        //LOCAL SOURCE COPY
        [Required] public ClientGetRequest ClientLocal { get; set;} = null!;
        [Required] public ContractorGetRequest ContractorLocal { get; set; } = null!;
        [Required] public UserAccountGetRequest UserAccountLocal { get; set; } = null!;
        
        [Required] public DateTime Maturity { get; set; }
        [Required] public DateTime Exposure { get; set; }
        [Required] public DateTime TaxableTransaction { get; set; }
        [Required] public DateTime Created { get; set; }

        //Reference 
        public virtual User User { get; set; } = null!; 
        public virtual ICollection<InvoiceService> InvoiceServices { get; set; } = null!;
        public virtual InvoiceTemplate? InvoiceTemplate { get; set; }
    }
}