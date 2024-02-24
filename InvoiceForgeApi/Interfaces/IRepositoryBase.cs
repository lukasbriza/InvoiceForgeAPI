using InvoiceForgeApi.Data.Enum;

namespace InvoiceForgeApi.Interfaces
{
    public interface IRepositoryBaseExtended<GET,ADD,UPDATE>: IRepositoryBase<GET,ADD,UPDATE>
    {
        Task<List<GET>?> GetAll(int userId);
    }
    public interface IRepositoryBaseWithClientExtended<GET,ADD,UPDATE>: IRepositoryBaseWithClient<GET,ADD,UPDATE>
    {
        Task<List<GET>?> GetAll(int userId);
    }

    public interface IRepositoryBase<GET,ADD,UPDATE>
    {
        Task<GET?> GetById(int entityId);
        Task<bool> Add(int userId, ADD entity);
        Task<bool> Update(int entityId, UPDATE entity);
        Task<bool> Delete(int entityId);
    }
    public interface IRepositoryBaseWithClient<GET,ADD,UPDATE>
    {
        Task<GET?> GetById(int entityId);
        Task<bool> Add(int userId, ADD entity, ClientType? type = null);
        Task<bool> Update(int entityId, UPDATE entity, ClientType? type = null);
        Task<bool> Delete(int entityId);
    }
}