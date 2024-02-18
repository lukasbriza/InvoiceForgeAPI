using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Interfaces
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetAll();
        Task<Bank?> GetById(int id);
        bool Add(Bank bank);
        bool Update(Bank bank);
        bool Delete(int id);
        bool Save();
    }
}
