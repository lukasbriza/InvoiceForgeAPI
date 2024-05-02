using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoiceTemplate
{
    public class AddInvoiceTemplateAbl: AblBase
    {
        public AddInvoiceTemplateAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userId, InvoiceTemplateAddRequest template)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    Client isClient = await IsInDatabase<Client>(template.ClientId, "Invalid client Id.");
                    if (isClient.Owner != userId) throw new ValidationError("Provided client is not in your possession.");

                    Contractor isContractor = await IsInDatabase<Contractor>(template.ContractorId, "Invalid contractor Id.");
                    if (isContractor.Owner != userId) throw new ValidationError("Provided contractor is not in your possession.");

                    UserAccount isUserAccount = await IsInDatabase<UserAccount>(template.UserAccountId, "Invalid user account Id.");
                    if (isUserAccount.Owner != userId) throw new ValidationError("Provided user account is not in your possession.");

                    var templateNameValidation = await _repository.InvoiceTemplate.GetByCondition(t => t.TemplateName == template.TemplateName && t.Owner == userId);
                    if (templateNameValidation is not null && templateNameValidation.Any()) throw new ValidationError("Template name must be unique.");

                    int? addInvoiceTemplate = await _repository.InvoiceTemplate.Add(userId, template);
                    bool saveCondition = addInvoiceTemplate is not null;

                    await SaveResult(saveCondition, transaction, false);
                    return saveCondition;
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