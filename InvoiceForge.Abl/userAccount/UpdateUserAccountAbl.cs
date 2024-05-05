using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.userAccount
{
    public class UpdateUserAccountAbl: AblBase
    {
        public UpdateUserAccountAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userAccountId, UserAccountUpdateRequest userAccount)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(userAccount.Owner);
                    UserAccount isUserAccount = await IsInDatabase<UserAccount>(userAccountId);

                    List<UserAccount>? accountNumberValidation = await _repository.UserAccount.GetByCondition(a => a.AccountNumber == userAccount.AccountNumber && a.Owner == userAccountId);
                    if (accountNumberValidation is not null && accountNumberValidation.Any()) throw new NotUniqueEntityError("Account number");

                    List<UserAccount>? ibanValidation = await _repository.UserAccount.GetByCondition(a => a.IBAN == userAccount.IBAN && a.Owner == userAccount.Owner);
                    if (ibanValidation is not null && ibanValidation.Any()) throw new NotUniqueEntityError("IBAN");

                    await IsInDatabase<Bank>(userAccount.BankId);

                    bool updateUserAccount = await _repository.UserAccount.Update(userAccountId, userAccount);
                    await SaveResult(updateUserAccount, transaction);
                    return updateUserAccount;
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