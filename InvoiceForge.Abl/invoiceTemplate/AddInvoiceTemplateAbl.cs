using InvoiceForgeApi.Errors;
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
                    Client isClient = await IsInDatabase<Client>(template.ClientId);
                    if (isClient.Owner != userId) throw new NoPossessionError();

                    Contractor isContractor = await IsInDatabase<Contractor>(template.ContractorId);
                    if (isContractor.Owner != userId) throw new NoPossessionError();

                    UserAccount isUserAccount = await IsInDatabase<UserAccount>(template.UserAccountId);
                    if (isUserAccount.Owner != userId) throw new NoPossessionError();

                    var templateNameValidation = await _repository.InvoiceTemplate.GetByCondition(t => t.TemplateName == template.TemplateName && t.Owner == userId);
                    if (templateNameValidation is not null && templateNameValidation.Any()) throw new NotUniqueEntityError("Template name");

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