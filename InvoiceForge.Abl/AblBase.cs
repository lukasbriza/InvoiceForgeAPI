using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InvoiceForgeApi.Abl
{
    public class AblBase: IAblBase
    {
        protected readonly IRepositoryWrapper _repository;
        public AblBase(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public virtual async Task<TEntity> IsInDatabase<TEntity>(int entityId) where TEntity: class
        {
            var dbSet = await _repository.GetSet<TEntity>();
            dbSet?.IgnoreAutoIncludes();

            if (dbSet is null) throw new DbSetError();

            var entity = await dbSet.FindAsync(entityId);
            if (entity is null) throw new NoEntityError();

            return entity;
        }
        public virtual async Task SaveResult(bool resultCondition, IDbContextTransaction transaction) => await SaveResult(resultCondition, transaction, true);
        public virtual async Task SaveResult(bool resultCondition, IDbContextTransaction transaction, bool saveRepository = true) {
            if (resultCondition == true) {
                if (saveRepository) await _repository.Save();
                await transaction.CommitAsync();
                return;
            };
            await transaction.RollbackAsync();
            return;
        }
    }
}