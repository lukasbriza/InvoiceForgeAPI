using System.Linq.Expressions;
using InvoiceForgeApi.Enum;

namespace InvoiceForgeApi.Interfaces
{
    public interface IIsUnique<TEntity>
    {
        Task<bool> IsUnique(int userId, TEntity entity);
    }
    public interface IBaseMethods<GET, TEntity>: IDelete
    {
        public Task<List<TEntity>?> GetByCondition(Expression<Func<TEntity,bool>> condition);
        Task<GET?> GetById(int entityId, bool? plain = false);
    }
    public interface IDelete
    {
        Task<bool> Delete(int entityId);
    }
    public interface IGetAll<TEntity>
    {
        Task<List<TEntity>?> GetAll(int userId, bool? plain = false);
    }
    public interface IAddSimple<TEntity>
    {
        Task<int?> Add(TEntity entity);
    }
    public interface IAdd<TEntity>
    {
        Task<int?> Add(int userId, TEntity entity);
    }
    public interface IAddClient<TEntity>
    {
        Task<int?> Add(int userId, TEntity entity, ClientType type);
    }
    public interface IUpdate<TEntity>
    {
        Task<bool> Update(int entityId, TEntity entity);
    }
    public interface IUpdateClient<TEntity>
    {
        Task<bool> Update(int entityId, TEntity entity, ClientType? type);
    }
}