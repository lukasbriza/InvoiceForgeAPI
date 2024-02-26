using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IUserAccountRepository: IRepositoryBaseExtended<UserAccountGetRequest, UserAccountAddRequest, UserAccountUpdateRequest, UserAccount> 
    {
        public Task<bool> HasDuplicitIbanOrAccountNumber(int userId, UserAccountAddRequest userAccount);
        private static Task<UserAccount?> Get(int id) => null!;
    }
}
