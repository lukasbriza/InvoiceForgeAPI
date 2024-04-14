using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
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
                    if (isUser.Id != isUserAccount.Owner) throw new ValidationError("Provided user account is not in your possession.");

                    if (userAccount.AccountNumber is not null)
                    {
                        List<UserAccount>? accountNumberValidation = await _repository.UserAccount.GetByCondition(a => a.AccountNumber == userAccount.AccountNumber && a.Owner == userAccountId);
                        if (accountNumberValidation is not null && accountNumberValidation.Any()) throw new ValidationError("Account number must be unique.");
                    }

                    if (userAccount.IBAN is not null)
                    {
                        List<UserAccount>? ibanValidation = await _repository.UserAccount.GetByCondition(a => a.IBAN == userAccount.IBAN && a.Owner == userAccount.Owner);
                        if (ibanValidation is not null && ibanValidation.Any()) throw new ValidationError("IBAN must be unique.");
                    }

                    if (userAccount.BankId is not null) await IsInDatabase<Bank>((int)userAccount.BankId, "Invalid bank Id.");

                    bool updateUserAccount = await _repository.UserAccount.Update(userAccountId, userAccount);
                    if (!updateUserAccount) throw new ValidationError("User account update failed.");

                    var invoices = await _repository.Invoice.GetByCondition(i => 
                        i.UserAccountLocal.Id == userAccountId && 
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