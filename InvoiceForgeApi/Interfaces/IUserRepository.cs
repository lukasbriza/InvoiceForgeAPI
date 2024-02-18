using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserRepository: IRepositoryBase<UserGetRequest, UserAddRequest, UserUpdateRequest>
    {
        private static Task<User?> Get(int id) => null!;
    }
}
