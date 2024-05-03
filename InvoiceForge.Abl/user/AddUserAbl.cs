using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.user
{
    public class AddUserAbl: AblBase
    {
        public AddUserAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(UserAddRequest user)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    int? addUser = await _repository.User.Add(user);
                    bool saveCondition = addUser is not null;

                    await SaveResult(saveCondition, transaction, false);
                    return saveCondition;
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