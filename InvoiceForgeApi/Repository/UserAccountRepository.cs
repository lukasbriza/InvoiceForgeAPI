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
    public async Task<List<UserAccountGetRequest>?> GetAll(int userId)
    {
        var userAccounts = await _dbContext.UserAccount
            .Include(u => u.Bank)
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
            .Where(u => u.Owner == userId).ToListAsync();

        return userAccounts;
    }
    public async Task<UserAccountGetRequest?> GetById(int userAccountId)
    {
        var userAccount = await _dbContext.UserAccount
            .Include(u => u.Bank)
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
        
        if(userAccount.Count > 1)
        {
            throw new ValidationError("Something unexpected happened. There are more than one user account with this ID.");
        }
        return userAccount[0];
    }

    public async Task<bool> Add(int userId, UserAccountAddRequest userAccount)
    {
        if(userAccount is null)
        {
            throw new ValidationError("User account is not provided");
        }

        var isDuplicitIBANOrAccountNumber = await _dbContext.UserAccount
            .Where(u => u.Owner == userId && (u.IBAN == userAccount.IBAN || u.AccountNumber == userAccount.AccountNumber))
            .ToListAsync();

        if(isDuplicitIBANOrAccountNumber.Count > 0)
        {
            throw new ValidationError("Canot add account with duplicit number or IBAN");
        }

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
        if (userAccount is null)
        {
            throw new ValidationError("User acccoun is not provided.");
        }

        var localUserAccount = await Get(userAccountId);

        if (localUserAccount is null)
        {
            throw new DatabaseCallError("User account is not in database.");
        }

        if (userAccount.BankId is not null)
        {
            var bankControl = await _dbContext.Bank.FindAsync(userAccount.BankId);

            if (bankControl is null)
            {
                throw new ValidationError("Provided bank is not in our database");
            }
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
}
