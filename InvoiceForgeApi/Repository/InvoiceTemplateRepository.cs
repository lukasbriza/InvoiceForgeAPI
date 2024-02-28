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
        public async Task<List<InvoiceTemplateGetRequest>?> GetAll(int userId, bool? plain)
        {
            var templates = await _dbContext.InvoiceTemplate
                .Select(i => new InvoiceTemplateGetRequest
                    {
                        Id = i.Id,
                        Owner = i.Owner,
                        ClientId = i.ClientId,
                        ContractorId = i.ContractorId,
                        UserAccountId = i.UserAccountId,
                        TemplateName = i.TemplateName,
                        Created = i.Created
                    }
                ).Where(i => i.Owner == userId).ToListAsync();

            return templates;
        }
        public async Task<InvoiceTemplateGetRequest?> GetById(int templateId, bool? plain)
        {
            var template = await _dbContext.InvoiceTemplate
                .Select(i => new InvoiceTemplateGetRequest
                    {
                        Id = i.Id,
                        Owner = i.Owner,
                        ClientId = i.ClientId,
                        ContractorId = i.ContractorId,
                        UserAccountId = i.UserAccountId,
                        TemplateName = i.TemplateName,
                        Created = i.Created
                    }
                ).Where(i => i.Id == templateId).ToListAsync();
            
            if (template.Count > 1)
            {
                throw new DatabaseCallError("Something unexpected happened. There are more than one invoice template with this ID.");
            }

            return template[0];
        }
        public async Task<bool> Add(int userId, InvoiceTemplateAddRequest template)
        {
            var newInvoiceTemplate = new InvoiceTemplate
            {
                Owner = userId,
                ClientId = template.ClientId,
                ContractorId = template.ContractorId,
                UserAccountId = template.UserAccountId,
                TemplateName = template.TemplateName,
                Created = new DateTime().ToUniversalTime()
            };
            await _dbContext.InvoiceTemplate.AddAsync(newInvoiceTemplate);
            return true;
        }
        public async Task<bool> Update(int templateId, InvoiceTemplateUpdateRequest template)
        {
            var localTemplate = await Get(templateId);

            if (localTemplate is null) throw new DatabaseCallError("Invoice template is not provided.");

            localTemplate.ClientId = template.ClientId ?? localTemplate.ClientId;
            localTemplate.ContractorId = template.ContractorId ?? localTemplate.ContractorId;
            localTemplate.UserAccountId = template.UserAccountId ?? localTemplate.UserAccountId;
            localTemplate.TemplateName = template.TemplateName ?? localTemplate.TemplateName;
            return true;      
        }
        public async Task<bool> Delete(int id)
        {
            var template = await Get(id);
            if (template is null) throw new DatabaseCallError("Template is not in database.");

            _dbContext.InvoiceTemplate.Remove(template);
            return true;
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