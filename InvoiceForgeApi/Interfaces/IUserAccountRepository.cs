using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<IEnumerable<UserAccount>> GetAll(int? owner);
        Task<UserAccount?> GetbyId(int id);
        bool Delete(int id);
        bool Update(UserAccount user);
        bool Add(UserAccount user);
        bool Save();
    }
}
