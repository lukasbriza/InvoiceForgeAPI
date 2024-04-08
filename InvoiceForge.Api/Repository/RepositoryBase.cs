using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.Enum;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Interfaces;
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
    public abstract class RepositoryExtended<TEntity, TAddRequest>: RepositoryBase<TEntity> 
        where TEntity: class, IEntityId 
        where TAddRequest: class
    {
        public RepositoryExtended(InvoiceForgeDatabaseContext dbContext) : base(dbContext){}

        public virtual async Task<int?> Add(int userId, TAddRequest addRequest)
        {
        var newEntity = Activator.CreateInstance(typeof(TEntity), userId, addRequest) as TEntity;
        if(newEntity is null) throw new ValidationError("Dynamic entity creation failed.");
        
        var dbSet = _dbContext.Set<TEntity>();
        var entityAddResult = await dbSet.AddAsync(newEntity);
        if (entityAddResult.State == EntityState.Added) await _dbContext.SaveChangesAsync();

        var entity = entityAddResult.Entity as IEntityId;
        return entityAddResult.State == EntityState.Unchanged ? entity.Id : null;
        }
    }
    public abstract class RepositoryExtendedSimple<TEntity, TAddRequest>: RepositoryBase<TEntity>
        where TEntity: class, IEntityId
        where TAddRequest: class
    {
        public RepositoryExtendedSimple(InvoiceForgeDatabaseContext dbContext) : base(dbContext){}

        public virtual async Task<int?> Add(TAddRequest addRequest)
        {
            var newEntity = Activator.CreateInstance(typeof(TEntity), addRequest) as TEntity;
            if(newEntity is null) throw new ValidationError("Dynamic entity creation failed.");

            var dbSet = _dbContext.Set<TEntity>();
            var entityAddResult = await dbSet.AddAsync(newEntity);
            if (entityAddResult.State == EntityState.Added) await _dbContext.SaveChangesAsync();

            var entity = entityAddResult.Entity as IEntityId;
            return entityAddResult.State == EntityState.Unchanged ? entity.Id : null;
        }
    }
    public abstract class RepositoryExtendeClient<TEntity, TAddRequest>: RepositoryBase<TEntity>
        where TEntity: class, IEntityId
        where TAddRequest: class
    {
        public RepositoryExtendeClient(InvoiceForgeDatabaseContext dbContext) : base(dbContext){}

        public virtual async Task<int?> Add(int userId, TAddRequest addRequest, ClientType clientType)
        {
            var newEntity = Activator.CreateInstance(typeof(TEntity), userId, addRequest, clientType) as TEntity;
            if(newEntity is null) throw new ValidationError("Dynamic entity creation failed.");

            var dbSet = _dbContext.Set<TEntity>();
            var entityAddResult = await dbSet.AddAsync(newEntity);
            if (entityAddResult.State == EntityState.Added) await _dbContext.SaveChangesAsync();

            var entity = entityAddResult.Entity as IEntityId;
            return entityAddResult.State == EntityState.Unchanged ? entity.Id : null;
        }
    }
}