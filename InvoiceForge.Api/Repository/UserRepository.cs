using InvoiceForgeApi.Data;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using InvoiceForgeApi.DTO.Model;

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
                user
                .Include(u => u.Clients).ThenInclude(c => c.Address).ThenInclude(a => a!.Country)
                .Include(u => u.Addresses).ThenInclude(a => a.Country)
                .Include(u => u.Contractors).ThenInclude(c => c.Address).ThenInclude(a => a!.Country)
                .Include(u => u.UserAccounts).ThenInclude(u => u.Bank)
                .Include(u => u.InvoiceItems).ThenInclude(i => i.Tariff);
            }
            
            var userCall = await user.FindAsync(id);
            var userResult = new UserGetRequest(userCall, plain);
            return userCall is not null ? userResult : null;
        }
        public async Task<bool> Update(int userId, UserUpdateRequest user)
        {
            if (user is null) throw new ValidationError("User is not provided.");
            var localUser = await Get(userId);

            if (localUser == null) throw new DatabaseCallError("User is not in database.");
            var userWithNewAuthIdExists = await _dbContext.User
                .Where(u => u.AuthenticationId == user.AuthenticationId)
                .ToListAsync();

            if (userWithNewAuthIdExists.Count > 0) throw new ValidationError("Provided values are incorrect.");
            localUser.AuthenticationId = user.AuthenticationId;
            
            var update = _dbContext.Update(localUser);
            return update.State == EntityState.Modified; 
        }
    }
}
