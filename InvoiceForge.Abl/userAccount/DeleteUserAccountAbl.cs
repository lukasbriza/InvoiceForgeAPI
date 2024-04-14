using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.userAccount
{
    public class DeleteUserAccountAbl: AblBase
    {
        public DeleteUserAccountAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userAccountId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    List<InvoiceTemplate>? invoiceTemplateReference = await _repository.InvoiceTemplate.GetByCondition((t) => t.UserAccountId == userAccountId);
                    if (invoiceTemplateReference is not null && invoiceTemplateReference.Any()) throw new ValidationError("Can´t delete. Still assigned to some entity.");

                    bool deleteUserAccount = await _repository.UserAccount.Delete(userAccountId);

                    await SaveResult(deleteUserAccount, transaction);
                    return deleteUserAccount;
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