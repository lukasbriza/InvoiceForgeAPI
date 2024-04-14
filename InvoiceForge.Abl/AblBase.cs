using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace InvoiceForgeApi.Abl
{
    public class AblBase
    {
        protected readonly IRepositoryWrapper _repository;
        public AblBase(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public virtual async Task<bool> IsInDatabase<TEntity>(int entityId, string? errorMessage = null) where TEntity: class
        {
            var dbSet = await _repository.GetSet<TEntity>();
            var setName = typeof(TEntity).FullName;

            if (dbSet is null) throw new DatabaseCallError($"There is no {setName} dbSet in given context.");

            var entity = await dbSet.FindAsync(entityId);
            return entity is not null;
        }
        public virtual async Task SaveResult(bool resultCondition) => await SaveResult(resultCondition, null);
        public virtual async Task SaveResult(bool resultCondition, IDbContextTransaction? transaction) {
            if (resultCondition) {
                await _repository.Save();
                if(transaction is not null) await transaction.CommitAsync();
                return;
            };

            if(transaction is not null) await transaction.RollbackAsync();
            return;
        }
    }
}