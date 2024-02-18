using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetlAll(int? owner);
        Task<Address?> GetById(int id);
        bool Add(Address address);
        bool Update(Address address);
        bool Delete(int id);
        bool Save();
    }
}
