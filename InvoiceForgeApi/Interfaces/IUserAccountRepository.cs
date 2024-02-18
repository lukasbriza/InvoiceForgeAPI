using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserAccountRepository: IRepositoryBaseExtended<UserAccountGetRequest, UserAccountAddRequest, UserAccountUpdateRequest> 
    {
        private static Task<UserAccount?> Get(int id) => null!;
    }
}
