using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoiceTemplate
{
    public class UpdateInvoiceTemplateAbl: AblBase
    {
        public UpdateInvoiceTemplateAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int templateId, InvoiceTemplateUpdateRequest template)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(template.Owner);
                    InvoiceTemplate isTemplate = await IsInDatabase<InvoiceTemplate>(templateId);
                    if (isUser.Id != isTemplate.Owner) throw new NoPossessionError();

                    Client isClient = await IsInDatabase<Client>(template.ClientId);
                    if (isClient.Owner != isUser.Id) throw new NoPossessionError();

                    Contractor isContractor = await IsInDatabase<Contractor>(template.ContractorId);
                    if (isContractor.Owner != isUser.Id) throw new NoPossessionError();

                    UserAccount isUserAccount = await IsInDatabase<UserAccount>(template.UserAccountId);
                    if (isUserAccount.Owner != isUser.Id) throw new NoPossessionError(); 

                    await IsInDatabase<Currency>(template.CurrencyId);
                    await IsInDatabase<Numbering>(template.NumberingId);
                    
                    var templateNamevalidation = await _repository.InvoiceTemplate.GetByCondition(t => 
                        t.TemplateName == template.TemplateName && 
                        t.Owner == isUser.Id
                    );
                    if (templateNamevalidation is not null && templateNamevalidation.Any()) throw new NotUniqueEntityError("Template name");

                    bool invoiceTemplateUpdate = await _repository.InvoiceTemplate.Update(templateId, template);
                    await SaveResult(invoiceTemplateUpdate, transaction);
                    return invoiceTemplateUpdate;
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