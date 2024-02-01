using InvoiceForgeApi.Handlers;
using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserRepository
    {
        Task<RequestHandler<User?>> GetById(int id);
        Task<RequestHandler<bool>> Delete(int id);
        Task<RequestHandler<bool>> Update(UserUpdateDTO user);
        Task<RequestHandler<bool>> Add(UserAddDTO user);
        private static Task<RequestHandler<bool>> Save() => null;
    }
}
