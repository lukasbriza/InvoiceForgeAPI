using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.user
{
    public class UpdateUserAbl: AblBase
    {
        public UpdateUserAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(UserUpdateRequest user)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    await IsInDatabase<User>(user.Id, "Invalid user Id.");

                    bool updateUser = await _repository.User.Update(user.Id, user);

                    await SaveResult(updateUser, transaction);
                    return updateUser;
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