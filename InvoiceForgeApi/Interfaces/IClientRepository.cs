

namespace InvoiceForgeApi.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAll(int? owner );
        Task<Client?> GetById(int id);
        bool Add(Client client);
        bool Update(Client client);
        bool Delete(int id);
        bool Save();
    }
}
