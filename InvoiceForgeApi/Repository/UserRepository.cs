using InvoiceForgeApi.Data;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using InvoiceForgeApi.DTO.Model;
using System.Linq.Expressions;

namespace InvoiceForgeApi.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public UserRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

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
            
            var userList = await user.Select(u => new UserGetRequest
                    {
                        Id =  u.Id,
                        Contractors = plain == false ? u.Contractors.Select(c => new ContractorGetRequest(c, plain)) : null,
                        Clients = plain == false ? u.Clients.Select(c => new ClientGetRequest(c, plain)) : null,
                        UserAccounts = plain == false ? u.UserAccounts.Select(u => new UserAccountGetRequest(u, plain)) : null,
                        Addresses = plain == false ? u.Addresses.Select(a => new AddressGetRequest(a, plain)) : null,
                        InvoiceItems = plain == false ? u.InvoiceItems.Select(i => new InvoiceItemGetRequest(i, plain)) : null
                    }
                )
                .Where(u => u.Id == id).ToListAsync();

            if (userList.Count == 0)
            {
                throw new DatabaseCallError("User is not in database.");
            }
            return userList[0];
        }
        public async Task<bool> Delete(int id)
        {
            var user = await Get(id);
                
            if (user is null)
            {
                throw new DatabaseCallError("User is not in database.");
            }

            var entity = _dbContext.User.Remove(user);
            return entity.State == EntityState.Deleted;
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
        public async Task<int?> Add(int userId, UserAddRequest user)
        {

            if(user is null)
            {
                throw new ValidationError("User is not provided.");
            }

            var newUser = new User {AuthenticationId = user.AuthenticationId};
            var entity = await _dbContext.User.AddAsync(newUser);
            
            if (entity.State == EntityState.Added) await _dbContext.SaveChangesAsync();
            return entity.State == EntityState.Unchanged ? entity.Entity.Id : null;
        }
        private async Task<User?> Get(int id)
        {
            return await _dbContext.User.FindAsync(id);
        }
        public async Task<List<User>?> GetByCondition(Expression<Func<User, bool>> condition)
        {
            var result = await _dbContext.User.Where(condition).ToListAsync();
            return result;
        }

    }
}
