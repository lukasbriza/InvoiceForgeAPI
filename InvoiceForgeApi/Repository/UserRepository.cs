using InvoiceForgeApi.Data;
using InvoiceForgeApi.Handlers;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO;

namespace InvoiceForgeApi.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public UserRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RequestHandler<User?>> GetById(int id)
        {
            var RHandler = new RequestHandler<User?>();
            try
            {
                var user = await _dbContext.User.FindAsync(id);

                if (user == null)
                {
                    throw new DatabaseCallError("User does not exist.");
                }

                RHandler.SetData(user);
            }
            catch (Exception ex) { RHandler.AddError(ex); }

            return RHandler;
        }
        public async Task<RequestHandler<bool>> Delete(int id)
        {
            var RHandler = new RequestHandler<bool>();
            var user = await GetById(id);

            if (user.HasErrors())
            {
                RHandler.AddErrors(user.Exceptions);
                RHandler.SetData(false);
                return RHandler;
            }
            if (user.Data is not null)
            {
                _dbContext.User.Remove(user.Data);
                var SHandler = await Save();
                RHandler.SetData(SHandler.Data);

                if (SHandler.HasErrors() | !SHandler.Data )
                {
                    RHandler.AddErrors(SHandler.Exceptions);
                    return RHandler;
                }
            }
            return RHandler;

        }

        public async Task<RequestHandler<bool>> Update(UserUpdateDTO user)
        {
            var RHandler = new RequestHandler<bool>();
            try
            {
                if (user is null)
                {
                    RHandler.SetData(false);
                    throw new ValidationError("User is not provided.");
                }

                _dbContext.User.Update(user);
                var SHandler = await Save();
                RHandler.SetData(SHandler.Data);

                if (SHandler.HasErrors() | !SHandler.Data)
                {
                    RHandler.AddErrors(SHandler.Exceptions);
                    return RHandler;
                }
            }
            catch (Exception ex) { RHandler.AddError(ex); }
            return RHandler;
        }

        public async Task<RequestHandler<bool>> Add(UserAddDTO user)
        {
            var RHandler = new RequestHandler<bool>();
            try
            {
                if(user is null)
                {
                    RHandler.SetData(false);
                    throw new ValidationError("User is not provided.");
                }

                var newUser = new User {AuthenticationId = user.AuthenticationId};
                await _dbContext.User.AddAsync(newUser);
                var SHandler = await Save();
                RHandler.SetData(SHandler.Data);

                if (SHandler.HasErrors() | !SHandler.Data)
                {
                    RHandler.AddErrors(SHandler.Exceptions);
                    return RHandler;
                }
            }
            catch(Exception ex) { RHandler.AddError(ex); }
            return RHandler;
        }

        private async Task<RequestHandler<bool>> Save()
        {
            var RHandler = new RequestHandler<bool>();
            try
            {
                int save = await _dbContext.SaveChangesAsync();
                RHandler.SetData(save > 0);

                if (!RHandler.Data)
                {
                    throw new DatabaseCallError("User saving failed.");
                }
            }
            catch (Exception ex){ RHandler.AddError(ex); }
            return RHandler;
        }
    }
}
