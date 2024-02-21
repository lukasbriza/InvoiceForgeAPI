
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/user-account")]
    public class UserAccountController: ControllerBase
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryWrapper _repository;

        public UserAccountController(IRepositoryWrapper repository)
        {
            _userAccountRepository = repository.UserAccount;
            _userRepository = repository.User;
            _repository = repository;
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<UserAccountGetRequest>?> GetAllUserAccounts(int userId)
        {
            return await _userAccountRepository.GetAll(userId);
        }

        [HttpGet]
        [Route("{userAccountId}")]
        public async Task<UserAccountGetRequest?> GetByUserAccountId(int userAccountId)
        {
            return await _userAccountRepository.GetById(userAccountId);
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddUserAccount(int userId, UserAccountAddRequest userAccount)
        {
            var isValidOwner = await _userRepository.GetById(userId);
            
            if (isValidOwner is null) return false;

            var addUserAccount = await _userAccountRepository.Add(userId, userAccount);
            if (addUserAccount) await _repository.Save();
            return addUserAccount;
        }

        [HttpPut]
        [Route("{userAccountId}")]
        public async Task<bool> UpdateUserAccount(int userAccountId, UserAccountUpdateRequest userAccount)
        {
            var user = await _userRepository.GetById(userAccount.Owner);
            if (user is null) return false;

            var isOwnerOfUserAccount = user.UserAccounts.Where(a => a.Id == userAccountId);
            if(isOwnerOfUserAccount is null || isOwnerOfUserAccount.Count() != 1)
            {
                throw new ValidationError("Provided values are wrong.");
            }

            var userAccountUpdate = await _userAccountRepository.Update(userAccountId, userAccount);
            if (userAccountUpdate) await _repository.Save();
            return userAccountUpdate;
        }

        [HttpDelete]
        [Route("{userAccountId}")]
        public async Task<bool> DeleteUserAccount(int userAccountId)
        {
            var deleteUserAccount = await _userAccountRepository.Delete(userAccountId);
            if (deleteUserAccount) await _repository.Save();
            return deleteUserAccount;
        }
    }
}

