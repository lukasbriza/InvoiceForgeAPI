using System.Linq.Expressions;
using InvoiceForgeApi.Data.Enum;

namespace InvoiceForgeApi.Interfaces
{
    public interface IRepositoryBaseExtended<GET,ADD,UPDATE, TEntity>: IRepositoryBase<GET,ADD,UPDATE, TEntity>
    {
        Task<List<GET>?> GetAll(int userId, bool? plain = false);
    }
    public interface IRepositoryBaseWithClientExtended<GET,ADD,UPDATE, TEntity>: IRepositoryBaseWithClient<GET,ADD,UPDATE, TEntity>
    {
        Task<List<GET>?> GetAll(int userId, bool? plain = false);
    }

    public interface IRepositoryBase<GET,ADD,UPDATE, TEntity>: IBaseMethods<GET, TEntity>
    {
        Task<int?> Add(int userId, ADD entity);
        Task<bool> Update(int entityId, UPDATE entity);
    }
    public interface IRepositoryBaseWithClient<GET,ADD,UPDATE, TEntity>: IBaseMethods<GET, TEntity>
    {
        Task<int?> Add(int userId, ADD entity, ClientType type);
        Task<bool> Update(int entityId, UPDATE entity, ClientType? typy);
    }

    public interface IBaseMethods<GET, TEntity>
    {
        public Task<List<TEntity>?> GetByCondition(Expression<Func<TEntity,bool>> condition);
        Task<GET?> GetById(int entityId, bool? plain = false);
        Task<bool> Delete(int entityId);
    }
}