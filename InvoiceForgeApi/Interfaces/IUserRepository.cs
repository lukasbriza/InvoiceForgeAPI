using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserRepository: IRepositoryBase<UserGetRequest, UserAddRequest, UserUpdateRequest, User>
    {
        private static Task<User?> Get(int id) => null!;
    }
}
