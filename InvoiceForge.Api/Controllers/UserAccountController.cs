using InvoiceForgeApi.Abl.userAccount;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/user-account")]
    public class UserAccountController: BaseController
    {
        public UserAccountController(IRepositoryWrapper repository): base(repository) {}
        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<UserAccountGetRequest>?> GetAllUserAccounts(int userId)
        {
            return await _repository.UserAccount.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<UserAccountGetRequest>?> GetPlainAllUserAccounts(int userId)
        {
            return await _repository.UserAccount.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{userAccountId}")]
        public async Task<UserAccountGetRequest?> GetByUserAccountId(int userAccountId)
        {
            return await _repository.UserAccount.GetById(userAccountId);
        }
        [HttpGet]
        [Route("plain/{userAccountId}")]
        public async Task<UserAccountGetRequest?> GetPlainByUserAccountId(int userAccountId)
        {
            return await _repository.UserAccount.GetById(userAccountId);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddUserAccount(int userId, UserAccountAddRequest userAccount)
        {
            if(userAccount is null) throw new ValidationError("User account is not provided");
            var abl = new AddUserAccountAbl(_repository);
            var result = await abl.Resolve(userId, userAccount);
            return result;
        }

        [HttpPut]
        [Route("{userAccountId}")]
        public async Task<bool> UpdateUserAccount(int userAccountId, UserAccountUpdateRequest userAccount)
        {
            if (userAccount is null) throw new ValidationError("User acccoun is not provided.");
            var abl = new UpdateUserAccountAbl(_repository);
            var result = await abl.Resolve(userAccountId, userAccount);
            return result;
        }

        [HttpDelete]
        [Route("{userAccountId}")]
        public async Task<bool> DeleteUserAccount(int userAccountId)
        {
            var abl = new DeleteUserAccountAbl(_repository);
            var result = await abl.Resolve(userAccountId);
            return result;
        }
    }
}

