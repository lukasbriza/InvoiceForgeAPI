﻿using InvoiceForgeApi.Data;
using Microsoft.EntityFrameworkCore;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using InvoiceForgeApi.Errors;

namespace InvoiceForgeApi.Repository
{
    public class UserRepository: 
        RepositoryExtendedSimple<User, UserAddRequest>,
        IUserRepository
    {
        public UserRepository(InvoiceForgeDatabaseContext dbContext): base(dbContext) {}

        public async Task<UserGetRequest?> GetById(int id, bool? plain = false)
        {
            DbSet<User> user = _dbContext.User;
            if (plain == false)
            {
                await user.Include(u => u.Clients).ThenInclude(c => c.Address).ThenInclude(a => a!.Country)
                    .Include(u => u.InvoiceTemplates)
                    .Include(u => u.Addresses).ThenInclude(a => a.Country)
                    .Include(u => u.Contractors).ThenInclude(c => c.Address).ThenInclude(a => a!.Country)
                    .Include(u => u.UserAccounts).ThenInclude(u => u.Bank)
                    .Include(u => u.InvoiceItems).ThenInclude(i => i.Tariff)
                    .LoadAsync();
            }
            
            var userCall = await user.FindAsync(id);
            var userResult = new UserGetRequest(userCall, plain);
            return userCall is not null ? userResult : null;
        }
        public async Task<bool> Update(int userId, UserUpdateRequest user)
        {
            var localUser = await Get(userId);
            if (localUser == null) throw new NoEntityError();

            var userWithNewAuthIdExists = await _dbContext.User.Where(u => u.AuthenticationId == user.AuthenticationId).ToListAsync();
            if (userWithNewAuthIdExists.Count > 0) throw new OperationError("Provided values are incorrect.");
            
            localUser.AuthenticationId = user.AuthenticationId;
            
            var update = _dbContext.Update(localUser);
            return update.State == EntityState.Modified; 
        }
    }
}
