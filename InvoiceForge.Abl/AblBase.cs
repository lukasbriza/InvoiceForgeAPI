using InvoiceForgeApi.DTO;
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

        public virtual async Task<TEntity> IsInDatabase<TEntity>(int entityId, string? errorMessage = null) where TEntity: class
        {
            var dbSet = await _repository.GetSet<TEntity>();
            dbSet?.IgnoreAutoIncludes();
            
            var setName = typeof(TEntity).FullName;

            if (dbSet is null) throw new DatabaseCallError($"There is no {setName} dbSet in given context.");

            var entity = await dbSet.FindAsync(entityId);

            if (errorMessage is not null && entity is null) throw new DatabaseCallError(errorMessage);
            if (entity is null) throw new DatabaseCallError("Entity is not in database.");

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