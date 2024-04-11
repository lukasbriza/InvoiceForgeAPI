using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
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
            var addUserAccountResult = addUserAccount is not null;

            if (addUserAccountResult) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return addUserAccountResult;
        }

        [HttpPut]
        [Route("{userAccountId}")]
        public async Task<bool> UpdateUserAccount(int userAccountId, UserAccountUpdateRequest userAccount)
        {
            if (userAccount is null) throw new ValidationError("User acccoun is not provided.");
        
            var user = await _userRepository.GetById(userAccount.Owner);
            if (user is null) throw new ValidationError("Provided user does not exist.");

            var isOwnerOfUserAccount = user.UserAccounts?.Where(a => a.Id == userAccountId);
            if (isOwnerOfUserAccount is null || isOwnerOfUserAccount.Count() != 1) throw new ValidationError("Provided values are wrong.");

            if (userAccount.AccountNumber is not null)
            {
                var accountNumberValidation = await _userAccountRepository.GetByCondition(a => a.AccountNumber == userAccount.AccountNumber && a.Owner == userAccountId);
                if (accountNumberValidation is not null && accountNumberValidation.Count > 0) throw new ValidationError("Account number must be unique.");
            }

            if (userAccount.IBAN is not null)
            {
                var ibanValidation = await _userAccountRepository.GetByCondition(a => a.IBAN == userAccount.IBAN && a.Owner == userAccount.Owner);
                if (ibanValidation is not null && ibanValidation.Count > 0) throw new ValidationError("IBAN must be unique.");
            }

            if (userAccount.BankId is not null)
            {
                var bankControl = await _codeListRepository.GetBankById((int)userAccount.BankId);
                if (bankControl is null) throw new ValidationError("Provided bank is not in our database");   
            }

            var userAccountUpdate = await _userAccountRepository.Update(userAccountId, userAccount);

            if (userAccountUpdate) {
                //OUTDATE LINKED INVOICES
                var invoices = await _invoiceRepository.GetByCondition(i => i.UserAccountLocal.Id == userAccountId && i.Owner == user.Id && i.Outdated == false);
                if (invoices is not null && invoices.Count > 0)
                {
                    invoices.ConvertAll(i => {
                        i.Outdated = true;
                        return i;
                    });
                }
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return userAccountUpdate;
        }

        [HttpDelete]
        [Route("{userAccountId}")]
        public async Task<bool> DeleteUserAccount(int userAccountId)
        {
            var hasInvoiceTemplatesReference = await _invoiceTemplateRepository.GetByCondition((template) => template.UserAccountId == userAccountId);
            if (hasInvoiceTemplatesReference is not null && hasInvoiceTemplatesReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

            var deleteUserAccount = await _userAccountRepository.Delete(userAccountId);

            if (deleteUserAccount) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return deleteUserAccount;
        }
    }
}

