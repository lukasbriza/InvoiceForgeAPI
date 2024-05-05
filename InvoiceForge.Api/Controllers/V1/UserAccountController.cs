using InvoiceForgeApi.Abl.userAccount;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
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
        public async Task<CustomResponse<List<UserAccountGetRequest>>> GetAllUserAccounts(int userId)
        {
            var task = await _repository.UserAccount.GetAll(userId);
            return CreateRepsonse(task ?? new List<UserAccountGetRequest>());
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<CustomResponse<List<UserAccountGetRequest>>> GetPlainAllUserAccounts(int userId)
        {
            var task = await _repository.UserAccount.GetAll(userId, true);
            return CreateRepsonse(task ?? new List<UserAccountGetRequest>());
        }
        [HttpGet]
        [Route("{userAccountId}")]
        public async Task<CustomResponse<UserAccountGetRequest?>> GetByUserAccountId(int userAccountId)
        {
            var task = await _repository.UserAccount.GetById(userAccountId);
            return CreateRepsonse(task);
        }
        [HttpGet]
        [Route("plain/{userAccountId}")]
        public async Task<CustomResponse<UserAccountGetRequest?>> GetPlainByUserAccountId(int userAccountId)
        {
            var task = await _repository.UserAccount.GetById(userAccountId, true);
            return CreateRepsonse(task);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<CustomResponse<bool>> AddUserAccount(int userId, UserAccountAddRequest userAccount)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new AddUserAccountAbl(_repository);
            var result = await abl.Resolve(userId, userAccount);
            return CreateRepsonse(result);
        }

        [HttpPut]
        [Route("{userAccountId}")]
        public async Task<CustomResponse<bool>> UpdateUserAccount(int userAccountId, UserAccountUpdateRequest userAccount)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new UpdateUserAccountAbl(_repository);
            var result = await abl.Resolve(userAccountId, userAccount);
            return CreateRepsonse(result);
        }

        [HttpDelete]
        [Route("{userAccountId}")]
        public async Task<CustomResponse<bool>> DeleteUserAccount(int userAccountId)
        {
            var abl = new DeleteUserAccountAbl(_repository);
            var result = await abl.Resolve(userAccountId);
            return CreateRepsonse(result);
        }
    }
}

