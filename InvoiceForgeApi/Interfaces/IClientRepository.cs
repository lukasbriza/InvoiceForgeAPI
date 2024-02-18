using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IClientRepository
    {
        Task<List<ClientGetRequest>> GetAll(int owner );
        Task<ClientGetRequest> GetById(int clientId);
        Task<bool> Add(int userId,ClientAddRequest client);
        Task<bool> Update(int clientId, ClientUpdateRequest client);
        Task<bool> Delete(int id);
        private static Task<bool> Save() => null;
        private static Task<User?> Get(int id) => null;
    }
}
