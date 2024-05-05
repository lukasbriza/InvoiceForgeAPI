using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.user
{
    public class DeleteUserAbl: AblBase
    {
        public DeleteUserAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    bool deleteUser = await _repository.User.Delete(userId);

                    await SaveResult(deleteUser, transaction);
                    return deleteUser;
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