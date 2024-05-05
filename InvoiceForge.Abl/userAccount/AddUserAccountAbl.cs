using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.userAccount
{
    public class AddUserAccountAbl: AblBase
    {
        public AddUserAccountAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userId, UserAccountAddRequest userAccount)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(userId);

                    bool isDuplicitIbanOrAccountNumber = await _repository.UserAccount.HasDuplicitIbanOrAccountNumber(userId, userAccount);
                    if (isDuplicitIbanOrAccountNumber) throw new NotUniqueEntityError("IBAN and account number");

                    int? addUserAccount = await _repository.UserAccount.Add(userId, userAccount);
                    bool saveCondition = addUserAccount is not null;

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