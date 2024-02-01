using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IInvoiceTemplateRepository
    {
        Task<IEnumerable<InvoiceTemplate>> GetAll(int? owner);
        Task<InvoiceTemplate?> GetById(int? owner);
        bool Delete(int id);
        bool Update(InvoiceTemplate user);
        bool Add(InvoiceTemplate user);
        bool Save();
    }
}
