using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
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

                    if (template.ClientId is not null)
                    {
                        Client isClient = await IsInDatabase<Client>((int)template.ClientId, "Invalid client Id.");
                        if (isClient.Owner != isUser.Id) throw new ValidationError("Provided client is not in your possession.");
                    }

                    if (template.ContractorId is not null)
                    {
                        Contractor isContractor = await IsInDatabase<Contractor>((int)template.ContractorId, "Invalid contractor Id.");
                        if (isContractor.Owner != isUser.Id) throw new ValidationError("Provided contractor is not in your possession.");
                    }

                    if (template.UserAccountId is not null)
                    {
                        UserAccount isUserAccount = await IsInDatabase<UserAccount>((int)template.UserAccountId, "Invalid user account Id.");
                        if (isUserAccount.Owner != isUser.Id) throw new ValidationError("Provided user account is not in your possession."); 
                    }

                    if (template.CurrencyId is not null) await IsInDatabase<Currency>((int)template.CurrencyId, "Invalid currency Id.");
                    
                    if (template.TemplateName is not null)
                    {
                        var templateNamevalidation = await _repository.InvoiceTemplate.GetByCondition(t => t.TemplateName == template.TemplateName && t.Owner == isUser.Id);
                        if (templateNamevalidation is not null && templateNamevalidation.Any()) throw new ValidationError("Template name must be unique.");
                    }

                    bool invoiceTemplateUpdate = await _repository.InvoiceTemplate.Update(templateId, template);
                    if (!invoiceTemplateUpdate) throw new ValidationError("Update invoice template failed.");

                    var invoices = await _repository.Invoice.GetByCondition(i => 
                        i.TemplateId == templateId && 
                        i.Owner == isUser.Id && 
                        i.Outdated == false
                    );
                    if (invoices is not null && invoices.Any())
                    {
                        invoices.ConvertAll(i => {
                            i.Outdated = true;
                            return i;
                        });
                    }

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