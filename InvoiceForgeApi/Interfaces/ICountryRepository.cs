using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetlAll();
        Task<Country?> GetById(int id);
        bool Add(Country country);
        bool Update(Country country);
        bool Delete(int id);
        bool Save();
    }
}
