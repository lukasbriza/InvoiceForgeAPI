using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IContractorRepository
    {
        Task<IEnumerable<Contractor>> GetlAll(int? owner);
        Task<Contractor> GetlById(int id);
        bool Add(Contractor contractor);
        bool Update(Contractor contractor);
        bool Delete(int id);
        bool Save();
    }
}
