
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
        private readonly ICodeListsRepository _codeLists;
        private readonly IInvoiceTemplateRepository _invoiceTemplate;
        private readonly IRepositoryWrapper _repository;

        public UserAccountController(IRepositoryWrapper repository)
        {
            _userAccountRepository = repository.UserAccount;
            _userRepository = repository.User;
            _codeLists = repository.CodeLists;
            _invoiceTemplate = repository.InvoiceTemplate;
            _repository = repository;
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<UserAccountGetRequest>?> GetAllUserAccounts(int userId)
        {
            return await _userAccountRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<UserAccountGetRequest>?> GetPlainAllUserAccounts(int userId)
        {
            return await _userAccountRepository.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{userAccountId}")]
        public async Task<UserAccountGetRequest?> GetByUserAccountId(int userAccountId)
        {
            return await _userAccountRepository.GetById(userAccountId);
        }
        [HttpGet]
        [Route("plain/{userAccountId}")]
        public async Task<UserAccountGetRequest?> GetPlainByUserAccountId(int userAccountId)
        {
            return await _userAccountRepository.GetById(userAccountId);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddUserAccount(int userId, UserAccountAddRequest userAccount)
        {
            var user = await _userRepository.GetById(userId);
            if (user is null) throw new ValidationError("Provided invalid user.");
           
            if(userAccount is null) throw new ValidationError("User account is not provided");
            var isDuplicitIbanOrAccountNumber = await _userAccountRepository.HasDuplicitIbanOrAccountNumber(userId, userAccount);
            if (isDuplicitIbanOrAccountNumber) throw new ValidationError("There is already account with that IBAN or account number.");

            var addUserAccount = await _userAccountRepository.Add(userId, userAccount);
            if (addUserAccount) await _repository.Save();
            return addUserAccount;
        }

        [HttpPut]
        [Route("{userAccountId}")]
        public async Task<bool> UpdateUserAccount(int userAccountId, UserAccountUpdateRequest userAccount)
        {
            if (userAccount is null) throw new ValidationError("User acccoun is not provided.");
        
            var user = await _userRepository.GetById(userAccount.Owner);
            if (user is null) throw new ValidationError("Provided user does not exist.");

            var isOwnerOfUserAccount = user.UserAccounts.Where(a => a.Id == userAccountId);
            if(isOwnerOfUserAccount is null || isOwnerOfUserAccount.Count() != 1) throw new ValidationError("Provided values are wrong.");

            if (userAccount.BankId is not null)
            {
                var bankControl = await _codeLists.GetBankById((int)userAccount.BankId);
                if (bankControl is null) throw new ValidationError("Provided bank is not in our database");   
            }

            var userAccountUpdate = await _userAccountRepository.Update(userAccountId, userAccount);
            if (userAccountUpdate) await _repository.Save();
            return userAccountUpdate;
        }

        [HttpDelete]
        [Route("{userAccountId}")]
        public async Task<bool> DeleteUserAccount(int userAccountId)
        {
            var hasInvoiceTemplatesReference = await _invoiceTemplate.GetByCondition((template) => template.UserAccountId == userAccountId);
            if (hasInvoiceTemplatesReference is not null && hasInvoiceTemplatesReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteUserAccount = await _userAccountRepository.Delete(userAccountId);
            if (deleteUserAccount) await _repository.Save();
            return deleteUserAccount;
        }
    }
}

