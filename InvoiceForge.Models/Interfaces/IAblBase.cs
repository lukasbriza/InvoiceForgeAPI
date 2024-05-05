using Microsoft.EntityFrameworkCore.Storage;

namespace InvoiceForgeApi.Models.Interfaces
{
    public interface IAblBase
    {
        public Task<TEntity> IsInDatabase<TEntity>(int entityId) where TEntity: class;
        public Task SaveResult(bool resultCondition, IDbContextTransaction transaction);
    }
}