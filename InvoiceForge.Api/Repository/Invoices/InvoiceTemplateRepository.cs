using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceTemplateRepository: 
        RepositoryExtended<InvoiceTemplate, InvoiceTemplateAddRequest>, 
        IInvoiceTemplateRepository
    {
        public InvoiceTemplateRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}
        public async Task<List<InvoiceTemplateGetRequest>?> GetAll(int userId, bool? plain = false)
        {
            var templates = await _dbContext.InvoiceTemplate
                .Where(i => i.Owner == userId)
                .ToListAsync();

            return templates.ConvertAll(i => new InvoiceTemplateGetRequest(i, plain));
        }
        public async Task<InvoiceTemplateGetRequest?> GetById(int templateId, bool? plain = false)
        {
            var templateCall = await _dbContext.InvoiceTemplate.FindAsync(templateId);
            var templateResult = new InvoiceTemplateGetRequest(templateCall, plain);
            return templateCall is not null ? templateResult : null;
        }
        public async Task<bool> Update(int templateId, InvoiceTemplateUpdateRequest template)
        {
            var localTemplate = await Get(templateId);
            if (localTemplate is null) throw new DatabaseCallError("Invoice template is not provided.");

            var localSelect = new {
                localTemplate.ClientId,
                localTemplate.ContractorId,
                localTemplate.UserAccountId,
                localTemplate.TemplateName,
                localTemplate.NumberingId
            };
            var updateSelect = new {
                template.ClientId,
                template.ContractorId,
                template.UserAccountId,
                template.TemplateName,
                template.NumberingId
            };
            if (localSelect.Equals(updateSelect)) throw new ValidationError("One of properties must be different from actual ones.");

            localTemplate.ClientId = template.ClientId;
            localTemplate.ContractorId = template.ContractorId;
            localTemplate.UserAccountId = template.UserAccountId;
            localTemplate.TemplateName = template.TemplateName;
            localTemplate.NumberingId = template.NumberingId;
           
            var update = _dbContext.Update(localTemplate);
            return update.State == EntityState.Modified;     
        }
        public async Task<bool> IsUnique(int userId, InvoiceTemplateAddRequest template)
        {
            var isInDatabase = await _dbContext.InvoiceTemplate.AnyAsync((t) =>
                t.Owner == userId &&
                t.ClientId == template.ClientId &&
                t.ContractorId == template.ContractorId &&
                t.UserAccountId == template.UserAccountId &&
                t.CurrencyId == template.CurrencyId &&
                t.NumberingId == template.NumberingId
            );
            return !isInDatabase;
        }
    }
}