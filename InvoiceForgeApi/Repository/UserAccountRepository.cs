using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository;

public class UserAccountRepository: RepositoryExtended<UserAccount, UserAccountAddRequest>, IUserAccountRepository
{
    public UserAccountRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}
    public async Task<List<UserAccountGetRequest>?> GetAll(int userId, bool? plain)
    {
        DbSet<UserAccount> userAccounts = _dbContext.UserAccount;
        if (plain == false)
        {
            userAccounts.Include(u => u.Bank);
        }
        
        var userAccountsList = await userAccounts
            .Select( u => new UserAccountGetRequest(u, plain))
            .Where(u => u.Owner == userId)
            .ToListAsync();

        return userAccountsList;
    }
    public async Task<UserAccountGetRequest?> GetById(int userAccountId, bool? plain)
    {
        DbSet<UserAccount> userAccount = _dbContext.UserAccount;
        if (plain == false)
        {
            userAccount.Include(u => u.Bank);
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

        if (localUserAccount is null)
        {
            throw new DatabaseCallError("User account is not in database.");
        }

        localUserAccount.BankId = userAccount.BankId ?? localUserAccount.BankId;
        localUserAccount.AccountNumber = userAccount.AccountNumber ?? localUserAccount.AccountNumber;
        localUserAccount.IBAN = userAccount.IBAN ?? localUserAccount.IBAN;
        
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
