using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class NumberingRepository: INumberingRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public NumberingRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GenerateInvoiceNumber?> GenerateInvoiceNumber(int numberingId)
        {
            //GET NUMBERING
            var numbering = await Get(numberingId);
            if (numbering is null) throw new DatabaseCallError("Numbering with that id is not in database.");
            
            var numberingTemplate = numbering.NumberingTemplate;

            //GET INVOICE WITH MAXIMUM ORDER
            var maxOrderInvoice = _dbContext.Invoice
                .Select(i => new {i.OrderNumber, i.InvoiceNumber, i.NumberingId})
                .Where(i => i.NumberingId == numberingId)
                .MaxBy(i => i.OrderNumber);

            var invoiceNumberObject = new GenerateInvoiceNumber();

            //ASSIGN NUMBERS BASED ON TEMPLATE
            if (maxOrderInvoice is null)
            {
                invoiceNumberObject.invoiceOrder = 1;
            } else {
                invoiceNumberObject.invoiceOrder = maxOrderInvoice.OrderNumber + 1;
            }

            var invoiceNumber = new InvoiceNumberMatrix(numberingTemplate, maxOrderInvoice?.InvoiceNumber).GetNumbering();
            
            if (invoiceNumber.Resolved == false || invoiceNumber.InvoiceNumber is null) throw new ValidationError("Something unexpected happened in InvoicENumberMatrix.");
            if (invoiceNumber.Overflowed)
            {
                //ADD NUMBER VARIABLE TO TEMPLATE
               var addVariableResult = await ExtendNumberVariableForNumbering(numbering.Id);
               if (addVariableResult == false) throw new ValidationError("Extending numbering variable failed.");
            }
            invoiceNumberObject.invoiceNumber = invoiceNumber.InvoiceNumber;
            return invoiceNumberObject;
        }
        public async Task<int?> AddNumbering(int userId, NumberingAddRequest numbering)
        {
            var newNumbering = new Numbering
            {
                Owner = userId,
                NumberingTemplate = numbering.NumberingTemplate,
                NumberingPrefix = numbering.NumberingPrefix ?? null
            };

            var entity = await _dbContext.Numbering.AddAsync(newNumbering);
            
            if (entity.State == EntityState.Added) await _dbContext.SaveChangesAsync();
            return entity.State == EntityState.Unchanged ? entity.Entity.Id : null;
        }
        public async Task<bool> UpdateNumbering(int numberingId, NumberingUpdateRequest numbering)
        {
            var localNumbering = await Get(numberingId);

            if (localNumbering is null) throw new DatabaseCallError("Numbering is not in database.");

            localNumbering.NumberingTemplate = numbering.NumberingTemplate ?? localNumbering.NumberingTemplate;
            localNumbering.NumberingPrefix = numbering.NumberingPrefix ?? localNumbering.NumberingPrefix;

            var update = _dbContext.Update(localNumbering);
            return update.State == EntityState.Modified;    
        }
        public async Task<bool> DeleteNumbering(int numberingId)
        {
            var numbering = await Get(numberingId);

            if (numbering is null) throw new DatabaseCallError("Numbering is not in database.");
            var entity = _dbContext.Numbering.Remove(numbering);
            return entity.State == EntityState.Deleted;
        }
        public async Task<List<Numbering>?> GetByCondition(Expression<Func<Numbering,bool>> condition)
        {
            var result = await _dbContext.Numbering.Where(condition).ToListAsync();
            return result;
        }
        private async Task<Numbering?> Get(int id)
        {
            return await _dbContext.Numbering.FindAsync(id);
        }
        private async Task<bool> ExtendNumberVariableForNumbering (int numberingId)
        {
            var numbering = await Get(numberingId);
            if (numbering is null) throw new DatabaseCallError("There is no nubering with that id.");
            var lastNumberIndex = numbering.NumberingTemplate.FindLastIndex(v => v == NumberingVariable.Number);
            numbering.NumberingTemplate.Insert(lastNumberIndex, NumberingVariable.Number);
            
            return _dbContext.Entry(numbering).State == EntityState.Modified;
        }
    }
}