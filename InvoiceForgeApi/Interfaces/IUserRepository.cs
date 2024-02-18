using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserRepository
    {
        Task<UserGetRequest?> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Update(UserUpdateRequest user);
        Task<bool> Add(UserAddRequest user);
        private static Task<bool> Save() => null;
        private static Task<User?> Get(int id) => null;
    }
}
