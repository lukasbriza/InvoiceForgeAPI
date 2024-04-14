using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.Interfaces;
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
            var setName = typeof(TEntity).FullName;

            if (dbSet is null) throw new DatabaseCallError($"There is no {setName} dbSet in given context.");

            var entity = await dbSet.FindAsync(entityId);

            if (errorMessage is not null && entity is null) throw new ValidationError(errorMessage);
            if (entity is null) throw new DatabaseCallError("Entity is not in database.");

            return entity;
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