using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoiceTemplate
{
    public class DeleteInvoiceTemplateAbl: AblBase
    {
        public DeleteInvoiceTemplateAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int templateId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    List<Invoice>? invoiceReference = await _repository.Invoice.GetByCondition(i => i.TemplateId == templateId);
                    if (invoiceReference is not null && invoiceReference.Any()) throw new ValidationError("Can´t delete. Still assigned to some entity.");

                    bool deleteInvoiceTemplate = await _repository.InvoiceTemplate.Delete(templateId);
                    
                    await SaveResult(deleteInvoiceTemplate, transaction);
                    return deleteInvoiceTemplate;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}