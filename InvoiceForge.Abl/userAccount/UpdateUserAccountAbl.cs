using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.CodeLists;
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
                    User isUser = await IsInDatabase<User>(userAccount.Owner, "Invalid user Id.");
                    UserAccount isUserAccount = await IsInDatabase<UserAccount>(userAccountId, "Invalid user account Id.");

                    List<UserAccount>? accountNumberValidation = await _repository.UserAccount.GetByCondition(a => a.AccountNumber == userAccount.AccountNumber && a.Owner == userAccountId);
                    if (accountNumberValidation is not null && accountNumberValidation.Any()) throw new ValidationError("Account number must be unique.");

                    List<UserAccount>? ibanValidation = await _repository.UserAccount.GetByCondition(a => a.IBAN == userAccount.IBAN && a.Owner == userAccount.Owner);
                    if (ibanValidation is not null && ibanValidation.Any()) throw new ValidationError("IBAN must be unique.");

                    await IsInDatabase<Bank>(userAccount.BankId, "Invalid bank Id.");

                    bool updateUserAccount = await _repository.UserAccount.Update(userAccountId, userAccount);
                    if (!updateUserAccount) throw new ValidationError("User account update failed.");

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