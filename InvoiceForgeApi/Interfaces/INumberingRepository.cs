using System.Linq.Expressions;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface INumberingRepository
    {
        Task<GenerateInvoiceNumber?> GenerateInvoiceNumber(int numberingId);
        Task<int?> AddNumbering(int userId, NumberingAddRequest numbering);
        Task<bool> UpdateNumbering(int numberingId, NumberingUpdateRequest numbering);
        Task<bool> DeleteNumbering(int numberingId);
        Task<List<Numbering>?> GetByCondition(Expression<Func<Numbering,bool>> condition);
        private static Task<Numbering?> Get(int id) => null!;
    }
}