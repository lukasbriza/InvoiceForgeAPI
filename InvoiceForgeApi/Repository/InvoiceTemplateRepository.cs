using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class InvoiceTemplateRepository: IInvoiceTemplateRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public InvoiceTemplateRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
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
        public async Task<int?> Add(int userId, InvoiceTemplateAddRequest template)
        {
            var newInvoiceTemplate = new InvoiceTemplate
            {
                Owner = userId,
                ClientId = template.ClientId,
                ContractorId = template.ContractorId,
                UserAccountId = template.UserAccountId,
                TemplateName = template.TemplateName,
                Created = new DateTime().ToUniversalTime(),
                NumberingId = template.NumberingId
            };
            var entity = await _dbContext.InvoiceTemplate.AddAsync(newInvoiceTemplate);
            
            if (entity.State == EntityState.Added) await _dbContext.SaveChangesAsync();
            return entity.State == EntityState.Unchanged ? entity.Entity.Id : null;
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
        public async Task<bool> Delete(int id)
        {
            var template = await Get(id);
            if (template is null) throw new DatabaseCallError("Template is not in database.");

            var entity = _dbContext.InvoiceTemplate.Remove(template);
            return entity.State == EntityState.Deleted;
        }
        private async Task<InvoiceTemplate?> Get(int id)
        {
            return await _dbContext.InvoiceTemplate.FindAsync(id);
        }
        public async Task<List<InvoiceTemplate>?> GetByCondition(Expression<Func<InvoiceTemplate,bool>> condition)
        {
            var result = await _dbContext.InvoiceTemplate.Where(condition).ToListAsync();
            return result;
        }
    }
}