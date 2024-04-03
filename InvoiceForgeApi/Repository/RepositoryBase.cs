using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public abstract class RepositoryBase<TEntity> where TEntity: class
    {
        public readonly InvoiceForgeDatabaseContext _dbContext;


        public RepositoryBase(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task<bool> Delete(int id)
        {
            var entity = await Get(id);
            var typeToString = entity?.GetType().FullName;
            if (entity is null) throw new DatabaseCallError($"{typeToString} is not in database.");

            var dbSet = _dbContext.Set<TEntity>();
            var removeResult = dbSet.Remove(entity);
            return removeResult.State == EntityState.Deleted;
        }
        public virtual async Task<TEntity?> Get(int id)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>?> GetByCondition(Expression<Func<TEntity,bool>> condition)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return await dbSet.Where(condition).ToListAsync();
        }
    }
}