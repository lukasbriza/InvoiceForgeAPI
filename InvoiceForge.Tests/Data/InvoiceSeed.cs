using InvoiceForgeApi.Models;
using InvoiceForgeApi.Repository;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class InvoiceSeed
    {
        InvoiceForgeDatabaseContext _context;
        public InvoiceSeed(InvoiceForgeDatabaseContext context)
        {
            _context = context;
        }
        
        public async  Task Populate()
        {   
            var repository = new RepositoryWrapper(_context);
            var num = await repository.Numbering.GenerateInvoiceNumber(1);

            var i1 = new Invoice(){
                    Owner = 1,
                    Outdated = false,
                    TemplateId = 1,
                    NumberingId = 1,
                    InvoiceNumber = num!.invoiceNumber,
                    OrderNumber = num.invoiceOrder,
                    BasePriceTotal = 1000,
                    VATTotal = 0,
                    Currency = "CZK",
                    ClientCopyId = 1,
                    ContractorCopyId = 1,
                    UserAccountCopyId = 1,
                    Maturity = DateTime.MaxValue,
                    Exposure = DateTime.MaxValue,
                    TaxableTransaction = DateTime.MaxValue,
                    Created = DateTime.Now
            };

            await _context.Invoice.AddAsync(i1);
            await _context.SaveChangesAsync();

            repository.DetachChanges();
            repository = new RepositoryWrapper(_context);
            var num2 = await repository.Numbering.GenerateInvoiceNumber(1);

            var i2 = new Invoice(){
                Owner = 1,
                Outdated = false,
                TemplateId = 1,
                NumberingId = 1,
                InvoiceNumber = num2!.invoiceNumber,
                OrderNumber = num2.invoiceOrder,
                BasePriceTotal = 1000,
                VATTotal = 0,
                Currency = "CZK",
                ClientCopyId = 1,
                ContractorCopyId = 1,
                UserAccountCopyId = 1,
                Maturity = DateTime.MaxValue,
                Exposure = DateTime.MaxValue,
                TaxableTransaction = DateTime.MaxValue,
                Created = DateTime.Now
            };

            await _context.Invoice.AddAsync(i2);
            await _context.SaveChangesAsync();
            repository.DetachChanges();
        }
    }
}