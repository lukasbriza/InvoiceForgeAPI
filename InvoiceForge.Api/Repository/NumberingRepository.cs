using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models.Enum;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using InvoiceForgeApi.Errors;

namespace InvoiceForgeApi.Repository
{
    public class NumberingRepository: 
        RepositoryExtended<Numbering, NumberingAddRequest>, 
        INumberingRepository
    {
        public NumberingRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}

        public async Task<GenerateInvoiceNumber?> GenerateInvoiceNumber(int numberingId)
        {
            //GET NUMBERING
            var numbering = await Get(numberingId);
            if (numbering is null) throw new NoEntityError();
            
            var numberingTemplate = numbering.NumberingTemplate;

            //GET INVOICE WITH MAXIMUM ORDER
            var maxOrderInvoice = await _dbContext.Invoice
            .Where(i => i.NumberingId == numberingId)
            .Select(i => new {i.OrderNumber, i.InvoiceNumber})
            .ToListAsync();

            var lastInvoice = maxOrderInvoice.MaxBy(i => i.OrderNumber);
            var invoiceNumberObject = new GenerateInvoiceNumber();

            //ASSIGN NUMBERS BASED ON TEMPLATE
            if (maxOrderInvoice is null || maxOrderInvoice.Count == 0)
            {
                invoiceNumberObject.invoiceOrder = 1;
            } else {
                invoiceNumberObject.invoiceOrder = lastInvoice!.OrderNumber + 1;
            }

            var invoiceNumber = new InvoiceNumberMatrix(numberingTemplate, lastInvoice?.InvoiceNumber).GetNumbering();
            
            if (invoiceNumber.Resolved == false || invoiceNumber.InvoiceNumber is null) throw new OperationError("Something unexpected happened in InvoiceNumberMatrix.");
            if (invoiceNumber.Overflowed)
            {
                //ADD NUMBER VARIABLE TO TEMPLATE
               var addVariableResult = await ExtendNumberVariableForNumbering(numbering.Id);
               if (addVariableResult == false) throw new OperationError("Extending numbering variable failed.");
            }
            invoiceNumberObject.invoiceNumber = invoiceNumber.InvoiceNumber;
            return invoiceNumberObject;
        }
        public async Task<bool> Update(int numberingId, NumberingUpdateRequest numbering)
        {
            var localNumbering = await Get(numberingId);
            if (localNumbering is null) throw new NoEntityError();

            var localSelect = new { localNumbering.NumberingTemplate, localNumbering.NumberingPrefix };
            var updateSelect = new { numbering.NumberingTemplate, numbering.NumberingPrefix };
            if (localSelect.Equals(updateSelect)) throw new EqualEntityError();

            localNumbering.NumberingTemplate = numbering.NumberingTemplate;
            localNumbering.NumberingPrefix = numbering.NumberingPrefix;

            var update = _dbContext.Update(localNumbering);
            return update.State == EntityState.Modified;    
        }

        private async Task<bool> ExtendNumberVariableForNumbering (int numberingId)
        {
            var numbering = await Get(numberingId);
            if (numbering is null) throw new NoEntityError();
            var lastNumberIndex = numbering.NumberingTemplate.FindLastIndex(v => v == NumberingVariable.Number);
            numbering.NumberingTemplate.Insert(lastNumberIndex, NumberingVariable.Number);
            
            var updateCall =  _dbContext.Update(numbering);
            var saveResult = await _dbContext.SaveChangesAsync();
            return saveResult > 0;
        }
    }
}