using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class UserAccountRepository: 
        RepositoryExtended<UserAccount, UserAccountAddRequest>, 
        IUserAccountRepository
    {
        public UserAccountRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}
        public async Task<List<UserAccountGetRequest>?> GetAll(int userId, bool? plain)
        {
            DbSet<UserAccount> userAccounts = _dbContext.UserAccount;
            if (plain == false)
            {
                await userAccounts.Include(u => u.Bank).LoadAsync();
            }
            
            var userAccountsList = await userAccounts
                .Where(u => u.Owner == userId)
                .ToListAsync();

            return userAccountsList.ConvertAll(u => new UserAccountGetRequest(u, plain));
        }
        public async Task<UserAccountGetRequest?> GetById(int userAccountId, bool? plain)
        {
            DbSet<UserAccount> userAccount = _dbContext.UserAccount;
            if (plain == false)
            {
                await userAccount.Include(u => u.Bank).LoadAsync();
            }
            
            var userAccountCall = await userAccount.FindAsync(userAccountId);
            var userAccountResult = new UserAccountGetRequest(userAccountCall, plain);
            return userAccountCall is not null ? userAccountResult : null;
        }
        public async Task<bool> HasDuplicitIbanOrAccountNumber(int userId, UserAccountAddRequest userAccount)
        {
            var isDuplicitIBANOrAccountNumber = await _dbContext.UserAccount
                .Where(u => u.Owner == userId && (u.IBAN == userAccount.IBAN || u.AccountNumber == userAccount.AccountNumber))
                .ToListAsync();
            if (isDuplicitIBANOrAccountNumber is null || isDuplicitIBANOrAccountNumber.Count() == 0) return false;
            return true;
        }
        public async Task<bool> Update(int userAccountId, UserAccountUpdateRequest userAccount)
        {
            var localUserAccount = await Get(userAccountId);
            if (localUserAccount is null) throw new DatabaseCallError("User account is not in database.");

            var localSelect = new { localUserAccount.BankId, localUserAccount.AccountNumber, localUserAccount.IBAN };
            var updateSelect = new { userAccount.BankId, userAccount.AccountNumber, userAccount.IBAN };
            if (localSelect.Equals(updateSelect)) throw new ValidationError("One of properties must be different from actual ones.");

            localUserAccount.BankId = userAccount.BankId;
            localUserAccount.AccountNumber = userAccount.AccountNumber;
            localUserAccount.IBAN = userAccount.IBAN;
            
            var update = _dbContext.Update(localUserAccount);
            return update.State == EntityState.Modified; 
        }
        public async Task<bool> IsUnique(int userId, UserAccountAddRequest account)
            {
                var isInDatabase = await _dbContext.UserAccount.AnyAsync((a) =>
                    a.Owner == userId &&
                    a.BankId == account.BankId &&
                    a.AccountNumber == account.AccountNumber &&
                    a.IBAN == account.IBAN
                );
                return !isInDatabase;
            }
    }
}


