using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
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
                .Select(i => new InvoiceTemplateGetRequest(i, plain))
                .Where(i => i.Owner == userId)
                .ToListAsync();

            return templates;
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

            localTemplate.ClientId = template.ClientId ?? localTemplate.ClientId;
            localTemplate.ContractorId = template.ContractorId ?? localTemplate.ContractorId;
            localTemplate.UserAccountId = template.UserAccountId ?? localTemplate.UserAccountId;
            localTemplate.TemplateName = template.TemplateName ?? localTemplate.TemplateName;
            localTemplate.NumberingId = template.NumberingId ?? localTemplate.NumberingId;
           
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