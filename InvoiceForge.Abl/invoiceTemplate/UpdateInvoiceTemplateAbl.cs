using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.CodeLists;
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
                    User isUser = await IsInDatabase<User>(template.Owner, "Invalid user Id.");
                    InvoiceTemplate isTemplate = await IsInDatabase<InvoiceTemplate>(templateId, "Invalid template Id.");
                    if (isUser.Id != isTemplate.Owner) throw new ValidationError("Template is not in your possession.");

                    Client isClient = await IsInDatabase<Client>(template.ClientId, "Invalid client Id.");
                    if (isClient.Owner != isUser.Id) throw new ValidationError("Provided client is not in your possession.");

                    Contractor isContractor = await IsInDatabase<Contractor>(template.ContractorId, "Invalid contractor Id.");
                    if (isContractor.Owner != isUser.Id) throw new ValidationError("Provided contractor is not in your possession.");

                    UserAccount isUserAccount = await IsInDatabase<UserAccount>(template.UserAccountId, "Invalid user account Id.");
                    if (isUserAccount.Owner != isUser.Id) throw new ValidationError("Provided user account is not in your possession."); 

                    await IsInDatabase<Currency>(template.CurrencyId, "Invalid currency Id.");
                    await IsInDatabase<Numbering>(template.NumberingId, "Invlaid numbering Id.");
                    
                    var templateNamevalidation = await _repository.InvoiceTemplate.GetByCondition(t => 
                        t.TemplateName == template.TemplateName && 
                        t.Owner == isUser.Id
                    );
                    if (templateNamevalidation is not null && templateNamevalidation.Any()) throw new ValidationError("Template name must be unique.");

                    bool invoiceTemplateUpdate = await _repository.InvoiceTemplate.Update(templateId, template);
                    if (!invoiceTemplateUpdate) throw new ValidationError("Update invoice template failed.");

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