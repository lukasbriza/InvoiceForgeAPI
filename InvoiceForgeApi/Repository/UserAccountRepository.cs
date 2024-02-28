using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository;

public class UserAccountRepository: IUserAccountRepository
{
    private readonly InvoiceForgeDatabaseContext _dbContext;
    public UserAccountRepository(InvoiceForgeDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<UserAccountGetRequest>?> GetAll(int userId, bool? plain)
    {
        DbSet<UserAccount> userAccounts = _dbContext.UserAccount;
        if (plain == false)
        {
            userAccounts.Include(u => u.Bank);
        }
        
        var userAccountsList = await userAccounts
            .Select( u => new UserAccountGetRequest
                {
                    Id = u.Id,
                    Owner = u.Owner,
                    BankId = u.BankId,
                    IBAN = u.IBAN,
                    AccountNumber = u.AccountNumber,
                    Bank = plain == false ? new BankGetRequest
                    {
                        Id = u.Bank!.Id,
                        Value = u.Bank.Value,
                        Shortcut = u.Bank.Shortcut,
                        SWIFT = u.Bank.SWIFT
                    } : null
                }
            )
            .Where(u => u.Owner == userId).ToListAsync();

        return userAccountsList;
    }
    public async Task<UserAccountGetRequest?> GetById(int userAccountId, bool? plain)
    {
        DbSet<UserAccount> userAccount = _dbContext.UserAccount;
        if (plain == false)
        {
            userAccount.Include(u => u.Bank);
        }
        
        var userAccountList = await userAccount
            .Select( u => new UserAccountGetRequest
                {
                    Id = u.Id,
                    Owner = u.Owner,
                    BankId = u.BankId,
                    IBAN = u.IBAN,
                    AccountNumber = u.AccountNumber,
                    Bank = new BankGetRequest
                    {
                        Id = u.Bank!.Id,
                        Value = u.Bank.Value,
                        Shortcut = u.Bank.Shortcut,
                        SWIFT = u.Bank.SWIFT
                    }
                }
            )
            .Where(u => u.Id == userAccountId).ToListAsync();
        
        if(userAccountList.Count > 1)
        {
            throw new ValidationError("Something unexpected happened. There are more than one user account with this ID.");
        }
        return userAccountList[0];
    }
    public async Task<bool> HasDuplicitIbanOrAccountNumber(int userId, UserAccountAddRequest userAccount)
    {
        var isDuplicitIBANOrAccountNumber = await _dbContext.UserAccount
            .Where(u => u.Owner == userId && (u.IBAN == userAccount.IBAN || u.AccountNumber == userAccount.AccountNumber))
            .ToListAsync();
        if (isDuplicitIBANOrAccountNumber is null || isDuplicitIBANOrAccountNumber.Count() == 0) return false;
        return true;
    }
    public async Task<bool> Add(int userId, UserAccountAddRequest userAccount)
    {
        var newUserAccount = new UserAccount
        {
            Owner = userId,
            BankId = userAccount.BankId,
            AccountNumber = userAccount.AccountNumber,
            IBAN = userAccount.IBAN
        };

        await _dbContext.UserAccount.AddAsync(newUserAccount);
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
        return true;
    }
    public async Task<bool> Delete(int userAccountId)
    {
        var userAccount = await Get(userAccountId);

        if (userAccount is null)
        {
            throw new DatabaseCallError("User account is not in database.");
        }

        _dbContext.UserAccount.Remove(userAccount);
        return true;
    }
    private async Task<UserAccount?> Get(int id)
    {
        return await _dbContext.UserAccount.FindAsync(id);
    }
    public async Task<List<UserAccount>?> GetByCondition(Expression<Func<UserAccount, bool>> condition)
    {
        var result = await _dbContext.UserAccount.Where(condition).ToListAsync();
        return result;
    }
}
